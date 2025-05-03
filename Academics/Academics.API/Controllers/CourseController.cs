using Academics.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Academics.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CourseController : ControllerBase
	{
		private readonly AppDbContext _db;

		public CourseController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet("getAllCourses")]
		[ProducesResponseType(typeof(List<CourseGetDto>), 200)]
		public async Task<ActionResult<List<CourseGetDto>>> GetAllCourses()
		{
			var courses = await _db.Courses
				.Include(c => c.Teacher)
				.Select(c => new CourseGetDto
				{
					Id = c.Id,
					CourseTitle = c.CourseTitle,
					Description = c.Description,
					Hours = c.Hours,
					Price = c.Price,
					Stars = c.Stars,
					TeacherId = c.TeacherId,
					TeacherName = c.Teacher.FullName,
					Category = c.Category,
					Image = c.Image
				})
				.ToListAsync();

			return Ok(courses);
		}

		[HttpGet("getCourseById")]
		public async Task<ActionResult<CourseGetDto>> GetCourseById([FromQuery] int id)
		{
			var course = await _db.Courses
				.Include(c => c.Teacher)
				.Where(c => c.Id == id)
				.Select(c => new CourseGetDto
				{
					Id = c.Id,
					CourseTitle = c.CourseTitle,
					Description = c.Description,
					Hours = c.Hours,
					Price = c.Price,
					Stars = c.Stars,
					TeacherId = c.TeacherId,
					TeacherName = c.Teacher.FullName
				})
				.FirstOrDefaultAsync();

			if (course == null)
			{
				return NotFound(new { message = $"Курс с Id {id} не найден" });
			}

			return Ok(course);
		}

		[HttpPost("createCourse")]
		public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto courseDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var teacherExists = await _db.Teachers.AnyAsync(t => t.Id == courseDto.TeacherId);
			if (!teacherExists)
			{
				return BadRequest(new { message = $"Преподаватель с Id {courseDto.TeacherId} не найден" });
			}

			var course = new Course
			{
				TeacherId = courseDto.TeacherId,
				Hours = courseDto.Hours,
				Description = courseDto.Description,
				CourseTitle = courseDto.CourseTitle,
				Price = courseDto.Price,
				Stars = courseDto.Stars
			};
			try
			{
				_db.Courses.Add(course);
				await _db.SaveChangesAsync();
				return Ok(new { message = "Курс успешно добавлен", courseId = course.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при добавлении курса: " + ex.Message });
			}
		}

		[HttpPut("updateCourse/{id:int}")]
		public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course courseUpdateData)
		{
			if (id != courseUpdateData.Id)
			{
				return BadRequest(new { message = "Id из URL и Id из тела запроса не совпадают" });
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingCourse = await _db.Courses.FindAsync(id);

			if (existingCourse == null)
			{
				return NotFound(new { message = "Курс не найден" });
			}

			if (existingCourse.TeacherId != courseUpdateData.TeacherId)
			{
				var teacherExists = await _db.Teachers.AnyAsync(t => t.Id == courseUpdateData.TeacherId);
				if (!teacherExists)
				{
					return BadRequest(new { message = $"Преподаватель с Id {courseUpdateData.TeacherId} не найден" });
				}
				existingCourse.TeacherId = courseUpdateData.TeacherId;
			}

			existingCourse.CourseTitle = courseUpdateData.CourseTitle;
			existingCourse.Description = courseUpdateData.Description;
			existingCourse.Hours = courseUpdateData.Hours;
			existingCourse.Price = courseUpdateData.Price;
			existingCourse.Stars = courseUpdateData.Stars;

			try
			{
				await _db.SaveChangesAsync();
				return Ok(new { message = "Курс успешно обновлен" });
			}
			catch (DbUpdateConcurrencyException)
			{
				return Conflict(new { message = "Конфликт параллельного доступа при обновлении." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при обновлении курса: " + ex.Message });
			}
		}

		[HttpPatch("updateCourse/{id:int}")]
		public async Task<IActionResult> PatchCourse(int id, [FromBody] CoursePatchDto patchData)
		{
			if (patchData == null)
			{
				return BadRequest(new { message = "Данные для обновления не предоставлены" });
			}

			var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == id);
			if (course == null)
			{
				return NotFound(new { message = "Курс не найден" });
			}

			bool changed = false;

			if (patchData.CourseTitle != null)
			{
				course.CourseTitle = patchData.CourseTitle;
				changed = true;
			}
			if (patchData.Description != null)
			{
				course.Description = patchData.Description;
				changed = true;
			}
			if (patchData.Hours != null)
			{
				course.Hours = patchData.Hours;
				changed = true;
			}
			if (patchData.Price.HasValue)
			{
				course.Price = patchData.Price.Value;
				changed = true;
			}
			if (patchData.Stars.HasValue)
			{
				if (patchData.Stars.Value < 0 || patchData.Stars.Value > 5)
				{
					return BadRequest(new { message = "Значение Stars должно быть между 0 и 5." });
				}
				course.Stars = patchData.Stars.Value;
				changed = true;
			}
			if (patchData.TeacherId.HasValue)
			{
				var teacherExists = await _db.Teachers.AnyAsync(t => t.Id == patchData.TeacherId.Value);
				if (!teacherExists)
				{
					return BadRequest(new { message = $"Преподаватель с Id {patchData.TeacherId.Value} не найден" });
				}
				if (course.TeacherId != patchData.TeacherId.Value)
				{
					course.TeacherId = patchData.TeacherId.Value;
					changed = true;
				}
			}

			if (!changed)
			{
				return Ok(new { message = "Изменений не было применено" });
			}

			try
			{
				await _db.SaveChangesAsync();
				return Ok(new { message = "Курс частично обновлен" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при частичном обновлении курса: " + ex.Message });
			}
		}

		[HttpDelete("deleteCourse")]
		public async Task<IActionResult> DeleteCourse([FromQuery] int Id)
		{
			try
			{
				var data = await _db.Courses.FirstOrDefaultAsync(f => f.Id == Id);
				if (data == null)
				{
					return NotFound(new { message = "Курс не найден" });
				}

				_db.Courses.Remove(data);
				await _db.SaveChangesAsync();
				return Ok(new { message = "Курс успешно удален" });
			}
			catch (DbUpdateException dbEx)
			{
				return BadRequest(new { message = "Ошибка при удалении: Возможно, существуют связанные данные. " + dbEx.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при удалении курса: " + ex.Message });
			}
		}
	}
}
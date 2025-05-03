using Academics.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academics.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TeacherController : ControllerBase
	{
		private readonly AppDbContext _db;

		public TeacherController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet("getAllTeachers")]
		[ProducesResponseType(typeof(List<TeacherDto>), 200)]
		public async Task<ActionResult<List<TeacherDto>>> GetAllTeachers()
		{
			var teachers = await _db.Teachers
				.Include(t => t.Position)
				.Include(t => t.Courses)
				.Select(t => new TeacherDto
				{
					Id = t.Id,
					FullName = t.FullName,
					Description = t.Description,
					Photo = t.Photo,
					Position = new PositionDto
					{
						Id = t.Position.Id,
						Name = t.Position.Name,
						Description = t.Position.Description
					},
					Courses = t.Courses.Select(c => new CourseSummaryDto
					{
						Id = c.Id,
						Name = c.CourseTitle,
						Description = c.Description,
						Price = c.Price
					}).ToList()
				})
				.ToListAsync();

			return Ok(teachers);
		}

		[HttpGet("getTeacherById")]
		[ProducesResponseType(typeof(TeacherDto), 200)]
		[ProducesResponseType(404)]
		public async Task<ActionResult<TeacherDto>> GetTeacherById([FromQuery] int id)
		{
			var teacher = await _db.Teachers
				.Include(t => t.Position)
				.Include(t => t.Courses)
				.Where(t => t.Id == id)
				.Select(t => new TeacherDto
				{
					Id = t.Id,
					FullName = t.FullName,
					Description = t.Description,
					Photo = t.Photo,
					Position = new PositionDto
					{
						Id = t.Position.Id,
						Name = t.Position.Name,
						Description = t.Position.Description
					},
					Courses = t.Courses.Select(c => new CourseSummaryDto
					{
						Id = c.Id,
						Name = c.CourseTitle,
						Description = c.Description,
						Price = c.Price
					}).ToList()
				})
				.FirstOrDefaultAsync();

			if (teacher == null)
			{
				return NotFound(new { message = $"Преподаватель с Id {id} не найден" });
			}

			return Ok(teacher);
		}

		[HttpPost("createTeacher")]
		public async Task<IActionResult> CreateTeacher(
			[FromForm] string fullName,
			[FromForm] int positionId,
			[FromForm] string? description,
			IFormFile? photo)
		{
			if (string.IsNullOrWhiteSpace(fullName))
			{
				ModelState.AddModelError("fullName", "Имя преподавателя обязательно.");
			}
			if (positionId <= 0)
			{
				ModelState.AddModelError("positionId", "Некорректный Id должности.");
			}

			var positionExists = await _db.Positions.AnyAsync(p => p.Id == positionId);
			if (!positionExists)
			{
				ModelState.AddModelError("positionId", $"Должность с Id {positionId} не найдена.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			byte[]? photoBytes = null;
			if (photo != null && photo.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					await photo.CopyToAsync(memoryStream);
					photoBytes = memoryStream.ToArray();
				}
			}

			var teacher = new Teacher
			{
				FullName = fullName,
				PositionId = positionId,
				Description = description,
				Photo = photoBytes,
				CreatedAt = DateTime.Now,
				CreatedBy = "admin"
			};

			try
			{
				_db.Teachers.Add(teacher);
				await _db.SaveChangesAsync();
				return Ok(new { message = "Преподаватель успешно добавлен", teacherId = teacher.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при добавлении преподавателя: " + ex.Message });
			}
		}

		[HttpPatch("updateTeacher/{id:int}")]
		public async Task<IActionResult> PatchTeacher(int id, [FromBody] TeacherPatchDto patchData)
		{
			if (patchData == null)
			{
				return BadRequest(new { message = "Данные для обновления не предоставлены" });
			}

			var teacher = await _db.Teachers.FirstOrDefaultAsync(p => p.Id == id);
			if (teacher == null)
			{
				return NotFound(new { message = "Преподаватель не найден" });
			}

			bool changed = false;

			if (patchData.FullName != null)
			{
				teacher.FullName = patchData.FullName;
				changed = true;
			}
			if (patchData.Description != null)
			{
				teacher.Description = patchData.Description;
				changed = true;
			}
			if (patchData.Photo != null)
			{
				teacher.Photo = patchData.Photo;
				changed = true;
			}
			if (patchData.PositionId.HasValue)
			{
				var positionExists = await _db.Positions.AnyAsync(p => p.Id == patchData.PositionId.Value);
				if (!positionExists)
				{
					return BadRequest(new { message = $"Должность с Id {patchData.PositionId.Value} не найдена" });
				}
				if (teacher.PositionId != patchData.PositionId.Value)
				{
					teacher.PositionId = patchData.PositionId.Value;
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
				return Ok(new { message = "Преподаватель частично обновлен" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при частичном обновлении преподавателя: " + ex.Message });
			}
		}

		[HttpDelete("deleteTeacher")]
		public async Task<IActionResult> DeleteTeacher([FromQuery] int Id)
		{
			try
			{
				var data = await _db.Teachers
									.Include(t => t.Courses)
									.FirstOrDefaultAsync(f => f.Id == Id);

				if (data == null)
				{
					return NotFound(new { message = "Преподаватель не найден" });
				}

				if (data.Courses != null && data.Courses.Any())
				{
					return BadRequest(new { message = "Нельзя удалить преподавателя, так как у него есть назначенные курсы." });
				}

				_db.Teachers.Remove(data);
				await _db.SaveChangesAsync();
				return Ok(new { message = "Преподаватель успешно удален" });
			}
			catch (DbUpdateException dbEx)
			{
				return BadRequest(new { message = "Ошибка при удалении: Возможно, существуют связанные данные. " + dbEx.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при удалении преподавателя: " + ex.Message });
			}
		}
	}
}
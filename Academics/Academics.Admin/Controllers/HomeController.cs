using Academics.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Academics.Admin.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _db;
		private readonly UserManager<AppUser> _userManager;

		public HomeController(ILogger<HomeController> logger, AppDbContext db, UserManager<AppUser> userManager)
		{
			_logger = logger;
			_db = db;
			_userManager = userManager;

		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Position()
		{
			var positions = _db.Positions.ToList();
			return View(positions);
		}

		public IActionResult Teacher()
		{
			var teachers = _db.Teachers.Include(t => t.Position).ToList();
			return View(teachers);
		}

		public IActionResult EditPosition(int id)
		{
			if (id != 0)
			{
				var position = _db.Positions.Find(id);

				return View(position);
			}
			else
			{
				return View(new Position());
			}
		}

		[HttpPost]
		public IActionResult EditPosition(Position position)
		{
			if (position.Id != 0)
			{
				var _position = _db.Positions.Find(position.Id);
				_position.Name = position.Name;
				_position.Description = position.Description;

				_db.SaveChanges();
			}
			else
			{
				position.CreatedBy = "admin";
				position.CreatedAt = DateTime.Now;
				_db.Positions.Add(position);
				_db.SaveChanges();
			}

			return RedirectToAction("Position", "Home");
		}

		public IActionResult DeletePosition(int id)
		{
			var _position = _db.Positions.Find(id);
			if (_position != null)
			{
				_db.Positions.Remove(_position);
				_db.SaveChanges();
			}

			return RedirectToAction("Position", "Home");
		}

		public IActionResult EditTeacher(int id)
		{
			ViewBag.Positions = _db.Positions.ToList();
			if (id != 0)
			{
				var teacher = _db.Teachers.Find(id);

				return View(teacher);
			}
			else
			{
				return View(new Teacher());
			}
		}

		[HttpPost]
		public async Task<IActionResult> EditTeacher(Teacher teacher, IFormFile PhotoFile)
		{
			teacher.CreatedAt = DateTime.Now;
			teacher.CreatedBy = "admin";
			if (teacher.Id != 0)
			{
				var _teacher = _db.Teachers.Find(teacher.Id);
				if (_teacher != null)
				{
					_teacher.FullName = teacher.FullName;
					_teacher.Description = teacher.Description;
					_teacher.PositionId = teacher.PositionId;

					if (PhotoFile != null && PhotoFile.Length > 0)
					{
						using (var memoryStream = new MemoryStream())
						{
							await PhotoFile.CopyToAsync(memoryStream);
							_teacher.Photo = memoryStream.ToArray();
						}
					}

					_db.SaveChanges();
				}
			}
			else
			{
				if (PhotoFile != null && PhotoFile.Length > 0)
				{
					using (var memoryStream = new MemoryStream())
					{
						await PhotoFile.CopyToAsync(memoryStream);
						teacher.Photo = memoryStream.ToArray();
					}
				}

				_db.Teachers.Add(teacher);
				_db.SaveChanges();
			}

			return RedirectToAction("Teacher", "Home");
		}

		public IActionResult DeleteTeacher(int id)
		{
			var _teacher = _db.Teachers.Find(id);
			if (_teacher != null)
			{
				_db.Teachers.Remove(_teacher);
				_db.SaveChanges();
			}

			return RedirectToAction("Teacher", "Home");
		}

		// ѕросмотр списка курсов
		public IActionResult Course()
		{
			var courses = _db.Courses.Include(c => c.Teacher).ToList();
			return View(courses);
		}

		// ќткрытие формы добавлени€/редактировани€ курса
		public IActionResult EditCourse(int id)
		{
			ViewBag.Teachers = new SelectList(_db.Teachers.ToList(), "Id", "FullName");

			if (id != 0)
			{
				var course = _db.Courses.Find(id);
				return View(course);
			}
			else
			{
				return View(new Course());
			}
		}

		// —охранение курса (добавление или редактирование)
		[HttpPost]
		public async Task<IActionResult> EditCourse(Course course, IFormFile? imageFile)
		{
			if (course.Id != 0)
			{
				var _course = _db.Courses.Find(course.Id);
				if (_course != null)
				{
					_course.TeacherId = course.TeacherId;
					_course.Hours = course.Hours;
					_course.Description = course.Description;
					_course.CourseTitle = course.CourseTitle;
					_course.Price = course.Price;
					_course.Stars = course.Stars;
					_course.Category = course.Category;

					if (imageFile != null && imageFile.Length > 0)
					{
						using (var memoryStream = new MemoryStream())
						{
							await imageFile.CopyToAsync(memoryStream);
							_course.image = memoryStream.ToArray();
						}
					}

					await _db.SaveChangesAsync();
				}
			}
			else
			{
				if (imageFile != null && imageFile.Length > 0)
				{
					using (var memoryStream = new MemoryStream())
					{
						await imageFile.CopyToAsync(memoryStream);
						course.image = memoryStream.ToArray();
					}
				}

				_db.Courses.Add(course);
				await _db.SaveChangesAsync();
			}

			return RedirectToAction("Course", "Home");
		}

		public IActionResult DeleteCourse(int id)
		{
			var _course = _db.Courses.Find(id);
			if (_course != null)
			{
				_db.Courses.Remove(_course);
				_db.SaveChanges();
			}

			return RedirectToAction("Course", "Home");
		}


	}
}

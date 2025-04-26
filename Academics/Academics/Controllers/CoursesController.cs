using Academics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Academics.Controllers
{
	
	public class CoursesController : Controller
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IStringLocalizer<CoursesController> _localizer;
        public CoursesController(ILogger<CoursesController> logger, IStringLocalizer<CoursesController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        
		[Authorize]
		public IActionResult Index()
        {
            _logger.LogInformation("Сourses page visited");
            var courses = new List<Course>
            {
                new Course { 
                    ImageUrl = "images/course_1.jpg", 
                    Title = _localizer["title1"], 
                    Category = _localizer["mobile_app"], 
                    Price = 99.00m, 
                    Stars = 3,
                    Description = _localizer["description"],
                    CourseUrl = "Courses/CourseDetails" },
                new Course { 
                    ImageUrl = "images/course_2.jpg", 
                    Title = _localizer["title2"], 
                    Category = _localizer["mobile_app"], 
                    Price = 99.00m, 
                    Stars = 4,
                    Description = _localizer["description"], 
                    CourseUrl = "Courses/CourseDetails" },
                new Course { 
                    ImageUrl = "images/course_3.jpg", 
                    Title = _localizer["title3"], 
                    Category = _localizer["mobile_app"], 
                    Price = 99.00m, 
                    Stars = 4,
                    Description = _localizer["description"],
                    CourseUrl = "Courses/CourseDetails" },
                new Course { 
                    ImageUrl = "images/course_4.jpg", 
                    Title = _localizer["title4"], 
                    Category = _localizer["mobile_app"], 
                    Price = 99.00m, 
                    Stars = 5,
                    Description = _localizer["description"], 
                    CourseUrl = "Courses/CourseDetails" },
                new Course { 
                    ImageUrl = "images/course_5.jpg", 
                    Title = _localizer["title5"], 
                    Category = _localizer["mobile_app"], 
                    Price = 99.00m,
                    Stars = 3,
                    Description = _localizer["description"], 
                    CourseUrl = "Courses/CourseDetails" },
                new Course { 
                    ImageUrl = "images/course_6.jpg", 
                    Title = _localizer["title6"], 
                    Category = _localizer["mobile_app"], 
                    Price = 99.00m,
                    Stars = 5,
                    Description = _localizer["description"], 
                    CourseUrl = "Courses/CourseDetails" }
            };
            _logger.LogInformation("Loaded {courses} courses", courses.Count);
            return View(courses);
        }
		
		[Authorize]
		public IActionResult CourseDetails()
        {
            _logger.LogInformation("Сourse details page visited");
            return View();
        }
    }
}

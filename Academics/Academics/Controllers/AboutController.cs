using Academics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Academics.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<AboutController> _logger;
        private readonly IStringLocalizer<AboutController> _localizer;
        public AboutController(ILogger<AboutController> logger, IStringLocalizer<AboutController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("About page visited");
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    ImageUrl = "images/person_1.jpg",
                    Name = _localizer["name1"],
                    Position = _localizer["position"],
                    Description = _localizer["description"]
                },
                new Teacher
                {
                    ImageUrl = "images/person_2.jpg",
                    Name = _localizer["name2"],
                    Position = _localizer["position"],
                    Description = _localizer["description"]
                },
                new Teacher
                {
                    ImageUrl = "images/person_3.jpg",
                    Name = _localizer["name3"],
                    Position = _localizer["position"],
                    Description = _localizer["description"]
                },
                new Teacher
                {
                    ImageUrl = "images/person_4.jpg",
                    Name = _localizer["name1"],
                    Position = _localizer["position"],
                    Description = _localizer["description"]
                },
                new Teacher
                {
                    ImageUrl = "images/person_2.jpg",
                    Name = _localizer["name2"],
                    Position = _localizer["position"],
                    Description = _localizer["description"]
                },
                new Teacher
                {
                    ImageUrl = "images/person_3.jpg",
                    Name = _localizer["name3"],
                    Position = _localizer["position"],
                    Description = _localizer["description"]
                }
            };

            _logger.LogInformation("Loaded {teachers} teachers", teachers.Count);

            return View(teachers);
        }
    }
}

using Academics.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Academics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        [Route("")]
		public IActionResult Index()
        {
            _logger.LogInformation("Сourses page visited");

            var slides = new List<MainSlide>
            {
                new MainSlide("/images/hero_1.jpg", _localizer["academics_university"]),
                new MainSlide("/images/hero_1.jpg", _localizer["learn"])
            };

            _logger.LogInformation("Сourses page visited");

            return View(slides);
        }

		[Route("News")]
		public IActionResult News() 
        {
            _logger.LogInformation("News page visited");
            return View();
        }

        [HttpPost]
        public JsonResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            return Json(culture);
        }
    }
}

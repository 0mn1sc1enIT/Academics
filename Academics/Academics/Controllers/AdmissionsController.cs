using Microsoft.AspNetCore.Mvc;

namespace Academics.Controllers
{
    public class AdmissionsController : Controller
    {
        private readonly ILogger<AdmissionsController> _logger;
        public AdmissionsController(ILogger<AdmissionsController> logger)
        {
            _logger = logger;
        }

        [Route("Admissions")]
		public IActionResult Index()
        {
            _logger.LogInformation("Admissions page visited");
            return View();
        }
    }
}

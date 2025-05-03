using Academics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
		public async Task<IActionResult> Index()
		{
			_logger.LogInformation("Courses page visited");

			List<CourseGetDto> courseDtos = new List<CourseGetDto>();
			string apiBaseUrl = "http://api.satbayevproject.kz";

			using (var client = new HttpClient())
			{
				var jwt = Request.Cookies["JwtToken"];
				if (!string.IsNullOrEmpty(jwt))
				{
					client.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", jwt);
				}

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				try
				{
					using (var response = await client
						.GetAsync($"{apiBaseUrl}/api/Course/getAllCourses"))
					{
						if (response.IsSuccessStatusCode)
						{
							var result = await response.Content.ReadAsStringAsync();
							courseDtos = JsonConvert.DeserializeObject<List<CourseGetDto>>(result) ?? new List<CourseGetDto>();
							_logger.LogInformation("Loaded {coursesCount} courses from API", courseDtos.Count);
						}
						else
						{
							_logger.LogWarning("Failed to load courses from API. Status: {statusCode}", response.StatusCode);
							ViewData["ErrorMessage"] = $"Не удалось загрузить курсы. Статус от API: {response.StatusCode}";
						}
					}
				}
				catch (HttpRequestException ex)
				{
					_logger.LogError(ex, "Network error while connecting to Courses API.");
					ViewData["ErrorMessage"] = "Ошибка при подключении к API курсов.";
				}
				catch (Exception ex) 
				{
					_logger.LogError(ex, "Error processing course data.");
					ViewData["ErrorMessage"] = "Произошла ошибка при обработке данных курсов.";
				}
			}

			var courseViewModels = courseDtos.Select(dto => new CourseViewModel
			{
				Id = dto.Id,
				Title = dto.CourseTitle,
				Description = dto.Description,
				Price = dto.Price,
				Stars = dto.Stars,
				TeacherName = dto.TeacherName,
				Hours = dto.Hours, 

				Image = dto.Image,
				Category = dto.Category,
				CourseUrl = "Courses/CourseDetails"
			}).ToList();

			// Передаем список ViewModel в представление
			return View(courseViewModels);
		}

		[Authorize]
		public IActionResult CourseDetails()
        {
            _logger.LogInformation("Сourse details page visited");
            return View();
        }
    }
}

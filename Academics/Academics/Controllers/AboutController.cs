using Academics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

        [Route("AboutUs")]
		public async Task<IActionResult> Index()
		{
			List<TeacherDto> teacherDtos = new List<TeacherDto>();

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
						.GetAsync($"{apiBaseUrl}/api/Teacher/getAllTeachers"))
					{
						if (response.IsSuccessStatusCode)
						{
							var result = await response.Content.ReadAsStringAsync();

							teacherDtos = JsonConvert.DeserializeObject<List<TeacherDto>>(result) ?? new List<TeacherDto>();
						}
						else
						{
							ViewData["ErrorMessage"] = $"Не удалось загрузить преподавателей. Статус от API: {response.StatusCode}";
						}
					}
				}
				catch (HttpRequestException ex)
				{
					ViewData["ErrorMessage"] = "Ошибка при подключении к API преподавателей.";
				}
				catch (Exception ex)
				{
					ViewData["ErrorMessage"] = "Произошла ошибка при обработке данных преподавателей.";
				}
			}

			var teacherViewModels = teacherDtos.Select(dto => new TeacherViewModel
			{
				Id = dto.Id,
				FullName = dto.FullName,
				Description = dto.Description,
				PositionName = dto.Position?.Name ?? "Должность не указана",
				PhotoBase64 = dto.Photo != null
							   ? $"data:image/jpeg;base64,{Convert.ToBase64String(dto.Photo)}"
							   : null
			}).ToList();

			return View(teacherViewModels);
		}
	}
}

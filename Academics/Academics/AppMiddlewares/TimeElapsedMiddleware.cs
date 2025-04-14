using System.Diagnostics;

namespace Academics.AppMiddlewares
{
	public class TimeElaspedMiddleware
	{
		private readonly ILogger<TimeElaspedMiddleware> _logger;
		private readonly RequestDelegate _next;
		public TimeElaspedMiddleware(RequestDelegate next, ILogger<TimeElaspedMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}
		public async Task Invoke(HttpContext context)
		{
			var timer = Stopwatch.StartNew();
			await _next(context);
			timer.Stop();
			var elapsedMs = timer.Elapsed.TotalMilliseconds;
			string page = context.Request.Path;
			string path = context.Request.Path;
			string method = context.Request.Method;
			string? user = context.User.Identity != null ? context.User.Identity.Name : "Guest";
			_logger.LogWarning("Регистрация запроса от пользователя {user}, по пути {path} метода {method}, отработала за {elapsedMs}", user, path, method, elapsedMs);
		}
	}
}
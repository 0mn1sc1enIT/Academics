namespace Academics.AppMiddlewares
{
	public class UseLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<UseLoggingMiddleware> _logger;
		public UseLoggingMiddleware(RequestDelegate next, ILogger<UseLoggingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			string path = context.Request.Path;
			string method = context.Request.Method;
			string? user = context.User.Identity != null ? context.User.Identity.Name : "Guest";
			_logger.LogWarning($"Регистрация запроса от пользователя {user}, по пути {path} метода {method}");
			await _next(context);
		}
	}
	public static class UseLoggingMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
		{
			return app.UseMiddleware<UseLoggingMiddleware>();
		}

	}
}

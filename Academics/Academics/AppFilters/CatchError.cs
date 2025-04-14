using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Academics.AppFilters
{
	public class CatchError : IExceptionFilter
	{
		private readonly ILogger<CatchError> _logger;
		public CatchError(ILogger<CatchError> logger)
		{
			_logger = logger;
		}
		public void OnException(ExceptionContext context)
		{
			_logger.LogError("Возникла ошибка: {errorMessage}", context.Exception.Message);
			context.Result = new RedirectToActionResult("Error", "Home", new { message = context.Exception.Message });
		}
	}
}

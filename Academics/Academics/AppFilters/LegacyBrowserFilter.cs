using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Academics.AppFilters
{
	public class LegacyBrowserFilterAttribute : Attribute, IResourceFilter
	{
		//Сразу после завершения этапа [Stage]
		public void OnResourceExecuted(ResourceExecutedContext context)
		{
		}
		//Выполняется непосредственно перед этапом Stage
		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			string userAgent = context.HttpContext.Request.Headers["user-agent"].ToString();
			if (userAgent.Contains("Edg"))
			{
				context.Result = new ContentResult
				{
					Content = "Ваш браузер устарел!"
				};
			}
		}
	}
}

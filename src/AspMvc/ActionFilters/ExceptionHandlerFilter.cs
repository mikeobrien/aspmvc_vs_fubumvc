using System.Web.Mvc;
using Core.Infrastructure.Logging;

namespace AspMvc.ActionFilters
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionHandlerFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogException(context.Exception);
            context.Result = new RedirectResult("/Error.htm");
            context.ExceptionHandled = true;
        }
    }
}
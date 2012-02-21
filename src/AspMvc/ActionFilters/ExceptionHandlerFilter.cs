using System.Net;
using System.Web.Mvc;
using Core.Domain;
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
            // Are you kidding me???
            var controllerDescriptor = (ControllerDescriptor)new ReflectedControllerDescriptor(context.Controller.GetType());
            var actionDescriptor = (ReflectedActionDescriptor)controllerDescriptor.FindAction(context, context.RouteData.Values["action"].ToString());

            if (actionDescriptor.MethodInfo.ReturnType == typeof(JsonResult))
            {
                var response = context.HttpContext.Response;
                if (context.Exception is ValidationException)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusDescription = context.Exception.Message;
                }
                else if (context.Exception is AuthorizationException)
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusDescription = "You are not authorized to perform this action.";
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusDescription = "A system error has occured.";
                    _logger.LogException(context.Exception);
                }
            }
            else
            {
                _logger.LogException(context.Exception);
                context.Result = new RedirectResult("/Error.htm");
            }
            context.ExceptionHandled = true;
        }
    }
}
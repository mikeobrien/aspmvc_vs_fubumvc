using System;
using System.Net;
using Core.Domain;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Web;
using FubuMVC.Core.Behaviors;

namespace FubuMvc.Behaviors
{
    public class AjaxExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly IHttpStatus _httpStatus;
        private readonly ILogger _logger;

        public AjaxExceptionHandlerBehavior(
            IActionBehavior innerBehavior,
            IHttpStatus httpStatus,
            ILogger logger)
        {
            _innerBehavior = innerBehavior;
            _httpStatus = httpStatus;
            _logger = logger;
        }

        public void Invoke()
        {
            try
            {
                _innerBehavior.Invoke();
            }
            catch (Exception e)
            {
                if (e is ValidationException) _httpStatus.Set(HttpStatusCode.BadRequest, e.Message);
                else if (e is AuthorizationException) 
                    _httpStatus.Set(HttpStatusCode.Unauthorized, "You are not authorized to perform this action.");
                else
                {
                    _httpStatus.Set(HttpStatusCode.InternalServerError, "A system error has occured.");
                    _logger.LogException(e);
                }
            }
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}

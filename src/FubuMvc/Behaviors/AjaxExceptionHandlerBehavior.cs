using System;
using System.Net;
using Core.Domain;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Web;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using HtmlTags;

namespace FubuMvc.Behaviors
{
    public class AjaxExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly IOutputWriter _outputWriter;
        private readonly ILogger _logger;

        public AjaxExceptionHandlerBehavior(
            IActionBehavior innerBehavior,
            IOutputWriter outputWriter,
            ILogger logger)
        {
            _innerBehavior = innerBehavior;
            _outputWriter = outputWriter;
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
                if (e is ValidationException) _outputWriter.WriteResponseCode(HttpStatusCode.BadRequest, e.Message);
                else if (e is AuthorizationException)
                    _outputWriter.WriteResponseCode(HttpStatusCode.Unauthorized, "You are not authorized to perform this action.");
                else
                {
                    _outputWriter.WriteResponseCode(HttpStatusCode.InternalServerError, "A system error has occured.");
                    _logger.LogException(e);
                }
                _outputWriter.Write(MimeType.Text, "");
            }
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}

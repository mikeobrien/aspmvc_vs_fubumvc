using System;
using Core.Infrastructure.Logging;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace FubuMvc.Behaviors
{
    public class ExceptionHandlerBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly ILogger _logger;
        private readonly IOutputWriter _outputWriter;

        public ExceptionHandlerBehavior(
            IActionBehavior innerBehavior,
            ILogger logger, 
            IOutputWriter outputWriter)
        {
            _innerBehavior = innerBehavior;
            _logger = logger;
            _outputWriter = outputWriter;
        }

        public void Invoke()
        {
            try
            {
                _innerBehavior.Invoke();
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                _outputWriter.RedirectToUrl("/Error.htm");
            }
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}

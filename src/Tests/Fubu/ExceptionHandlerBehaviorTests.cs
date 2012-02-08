using System;
using Core.Infrastructure.Logging;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMvc.Behaviors;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Fubu
{
    [TestFixture]
    public class ExceptionHandlerBehaviorTests
    {
        [Test]
        public void Should_Log_And_Redirect_To_The_Error_Page_When_There_Is_An_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var outputWriter = Substitute.For<IOutputWriter>();
            var logger = Substitute.For<ILogger>();
            var exception = new Exception("bad things happning");

            innerBehavior.When(x => x.Invoke()).Do(x => { throw exception; });

            var exceptionHandlerBehavior = new ExceptionHandlerBehavior(innerBehavior, logger, outputWriter);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            outputWriter.Received().RedirectToUrl("/Error.htm");
            logger.Received().LogException(exception);
        }

        [Test]
        public void Should_Not_Log_And_Redirect_To_The_Error_Page_When_There_Is_Not_An_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var outputWriter = Substitute.For<IOutputWriter>();
            var logger = Substitute.For<ILogger>();

            var exceptionHandlerBehavior = new ExceptionHandlerBehavior(innerBehavior, logger, outputWriter);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            outputWriter.DidNotReceiveWithAnyArgs().RedirectToUrl(null);
            logger.DidNotReceiveWithAnyArgs().LogException(null);
        }
    }
}
using System;
using System.Net;
using Core.Domain;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Web;
using FubuMVC.Core.Behaviors;
using FubuMvc.Behaviors;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Fubu
{
    [TestFixture]
    public class AjaxExceptionHandlerBehaviorTests
    {
        [Test]
        public void Should_Not_Do_Anything_If_There_Is_Not_An_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var httpStatus = Substitute.For<IHttpStatus>();
            var logger = Substitute.For<ILogger>();

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, httpStatus, logger);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            httpStatus.DidNotReceiveWithAnyArgs().Set(0);
            httpStatus.DidNotReceiveWithAnyArgs().Set(HttpStatusCode.OK);
            logger.DidNotReceiveWithAnyArgs().LogException(null);
        }

        [Test]
        public void Should_Log_And_Set_Status_To_500_When_There_Is_An_Unhandled_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var httpStatus = Substitute.For<IHttpStatus>();
            var logger = Substitute.For<ILogger>();
            var exception = new Exception("bad things happening");

            innerBehavior.When(x => x.Invoke()).Do(x => { throw exception; });

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, httpStatus, logger);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            httpStatus.Received().Set(HttpStatusCode.InternalServerError, "A system error has occured.");
            logger.Received().LogException(exception);
        }

        [Test]
        public void Should_Not_Log_And_Set_Status_To_401_When_There_Is_An_Authorization_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var httpStatus = Substitute.For<IHttpStatus>();
            var logger = Substitute.For<ILogger>();

            innerBehavior.When(x => x.Invoke()).Do(x => { throw new AuthorizationException(); });

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, httpStatus, logger);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            httpStatus.Received().Set(HttpStatusCode.Unauthorized, "You are not authorized to perform this action.");
            logger.DidNotReceiveWithAnyArgs().LogException(null);
        }

        [Test]
        public void Should_Not_Log_And_Set_Status_To_403_When_There_Is_A_Validation_Exception()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var httpStatus = Substitute.For<IHttpStatus>();
            var logger = Substitute.For<ILogger>();
            var exception = new ValidationException("why u enter bad data??");

            innerBehavior.When(x => x.Invoke()).Do(x => { throw exception; });

            var exceptionHandlerBehavior = new AjaxExceptionHandlerBehavior(innerBehavior, httpStatus, logger);

            exceptionHandlerBehavior.Invoke();

            innerBehavior.Received().Invoke();
            httpStatus.Received().Set(HttpStatusCode.BadRequest, exception.Message);
            logger.DidNotReceiveWithAnyArgs().LogException(null);
        }
    }
}
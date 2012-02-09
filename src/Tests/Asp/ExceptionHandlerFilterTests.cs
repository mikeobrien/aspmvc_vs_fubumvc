using System;
using System.Web.Mvc;
using AspMvc.ActionFilters;
using Core.Infrastructure.Logging;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Tests.Asp
{
    [TestFixture]
    public class ExceptionHandlerFilterTests
    {
        [Test]
        public void Should_Log_And_Redirect_To_The_Error_Page_When_There_Is_An_Exception()
        {
            var exceptionContext = Substitute.For<ExceptionContext>();
            var logger = Substitute.For<ILogger>();
            var exception = new Exception("something bad happened!!");

            exceptionContext.Exception.Returns(exception);

            var exceptionHandlerFilter = new ExceptionHandlerFilter(logger);

            exceptionHandlerFilter.OnException(exceptionContext);

            exceptionContext.ExceptionHandled.ShouldBeTrue();
            ((RedirectResult)exceptionContext.Result).Url.ShouldEqual("/Error.htm");
            logger.Received().LogException(exception);
        }
    }
}
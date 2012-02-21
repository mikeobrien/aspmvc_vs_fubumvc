using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AspMvc.ActionFilters;
using Core.Domain;
using Core.Infrastructure.Logging;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Tests.Asp
{
    [TestFixture]
    public class ExceptionHandlerFilterTests
    {
        public class DummyController : Controller
        {
            public ViewResult ViewAction() { return null; }
            public JsonResult JsonAction() { return null; }
        }

        [Test]
        public void Should_Log_And_Redirect_To_The_Error_Page_When_There_Is_An_Exception_With_An_Action_Result()
        {
            var exceptionContext = new ExceptionContext();
            var logger = Substitute.For<ILogger>();
            var exception = new Exception("something bad happened!!");

            // GRIPE: You cannot mock these properties
            exceptionContext.Result = new ViewResult();
            exceptionContext.Exception = exception;
            exceptionContext.Controller = new DummyController();
            exceptionContext.RouteData = new RouteData();
            exceptionContext.RouteData.Values["action"] = "viewaction";

            var exceptionHandlerFilter = new ExceptionHandlerFilter(logger);

            exceptionHandlerFilter.OnException(exceptionContext);

            exceptionContext.ExceptionHandled.ShouldBeTrue();
            ((RedirectResult)exceptionContext.Result).Url.ShouldEqual("/Error.htm");
            logger.Received().LogException(exception);
        }

        [Test]
        public void Should_Log_And_Set_Status_To_500_With_An_Json_Result_An_Unhandled_Exception()
        {
            var exceptionContext = new ExceptionContext();
            var logger = Substitute.For<ILogger>();
            var exception = new Exception("something bad happened!!");

            exceptionContext.HttpContext = Substitute.For<HttpContextBase>();
            exceptionContext.Result = new JsonResult();
            exceptionContext.Exception = exception;
            exceptionContext.Controller = new DummyController();
            exceptionContext.RouteData = new RouteData();
            exceptionContext.RouteData.Values["action"] = "jsonaction";

            var exceptionHandlerFilter = new ExceptionHandlerFilter(logger);

            exceptionHandlerFilter.OnException(exceptionContext);

            exceptionContext.HttpContext.Response.StatusCode.ShouldEqual(500);
            exceptionContext.HttpContext.Response.StatusDescription.ShouldEqual("A system error has occured.");
            exceptionContext.ExceptionHandled.ShouldBeTrue();
            logger.Received().LogException(exception);
        }

        [Test]
        public void Should_Not_Log_And_Only_Set_Status_To_401_With_An_Json_Result_And_Authorization_Exception()
        {
            var exceptionContext = new ExceptionContext();
            var logger = Substitute.For<ILogger>();

            exceptionContext.HttpContext = Substitute.For<HttpContextBase>();
            exceptionContext.Result = new JsonResult();
            exceptionContext.Exception = new AuthorizationException();
            exceptionContext.Controller = new DummyController();
            exceptionContext.RouteData = new RouteData();
            exceptionContext.RouteData.Values["action"] = "jsonaction";

            var exceptionHandlerFilter = new ExceptionHandlerFilter(logger);

            exceptionHandlerFilter.OnException(exceptionContext);

            exceptionContext.HttpContext.Response.StatusCode.ShouldEqual(401);
            exceptionContext.HttpContext.Response.StatusDescription.ShouldEqual("You are not authorized to perform this action.");
            exceptionContext.ExceptionHandled.ShouldBeTrue();
            logger.DidNotReceiveWithAnyArgs().LogException(null);
        }

        [Test]
        public void Should_Not_Log_And_Only_Set_Status_To_400_With_An_Json_Result_And_Validation_Exception()
        {
            var exceptionContext = new ExceptionContext();
            var logger = Substitute.For<ILogger>();
            var exception = new ValidationException("why u enter bad data??");

            exceptionContext.HttpContext = Substitute.For<HttpContextBase>();
            exceptionContext.Result = new JsonResult();
            exceptionContext.Exception = exception;
            exceptionContext.Controller = new DummyController();
            exceptionContext.RouteData = new RouteData();
            exceptionContext.RouteData.Values["action"] = "jsonaction";

            var exceptionHandlerFilter = new ExceptionHandlerFilter(logger);

            exceptionHandlerFilter.OnException(exceptionContext);

            exceptionContext.HttpContext.Response.StatusCode.ShouldEqual(400);
            exceptionContext.HttpContext.Response.StatusDescription.ShouldEqual(exception.Message);
            exceptionContext.ExceptionHandled.ShouldBeTrue();
            logger.DidNotReceiveWithAnyArgs().LogException(null);
        }
    }
}
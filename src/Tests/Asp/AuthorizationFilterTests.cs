using System.Web.Mvc;
using AspMvc.ActionFilters;
using Core.Domain;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Tests.Asp
{
    [TestFixture]
    public class AuthorizationFilterTests
    {
        [Test]
        public void Should_Invoke_The_Inner_Behavior_When_Logged_In_And_Authorized()
        {
            var authorizationContext = Substitute.For<AuthorizationContext>();
            var authorizationService = Substitute.For<IAuthorizationService>();

            authorizationService.IsLoggedIn.Returns(true);
            authorizationService.IsAuthorized.Returns(true);

            var authorizationFilter = new AuthorizationFilter(authorizationService);

            authorizationFilter.OnAuthorization(authorizationContext);

            authorizationContext.Result.ShouldBeNull();
        }

        [Test]
        public void Should_Redirect_To_Login_Page_When_Not_Logged_In()
        {
            var authorizationContext = Substitute.For<AuthorizationContext>();
            var authorizationService = Substitute.For<IAuthorizationService>();

            authorizationService.IsLoggedIn.Returns(false);
            authorizationService.IsAuthorized.Returns(false);

            var authorizationFilter = new AuthorizationFilter(authorizationService);

            authorizationFilter.OnAuthorization(authorizationContext);

            authorizationContext.Result.ShouldBeType<RedirectResult>();
            ((RedirectResult)authorizationContext.Result).Url.ShouldEqual("/login");
        }

        [Test]
        public void Should_Redirect_To_Access_Denied_Page_When_Not_Authorized()
        {
            var authorizationContext = Substitute.For<AuthorizationContext>();
            var authorizationService = Substitute.For<IAuthorizationService>();

            authorizationService.IsLoggedIn.Returns(true);
            authorizationService.IsAuthorized.Returns(false);

            var authorizationFilter = new AuthorizationFilter(authorizationService);

            authorizationFilter.OnAuthorization(authorizationContext);

            authorizationContext.Result.ShouldBeType<RedirectResult>();
            ((RedirectResult)authorizationContext.Result).Url.ShouldEqual("/AccessDenied.htm");
        }
    }
}
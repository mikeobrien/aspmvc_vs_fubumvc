using Core.Domain;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMvc.Behaviors;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Fubu
{
    [TestFixture]
    public class AuthorizationBehaviorTests
    {
        [Test]
        public void Should_Invoke_The_Inner_Behavior_When_Logged_In_And_Authorized()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var outputWriter = Substitute.For<IOutputWriter>();
            var authorizationService = Substitute.For<IAuthorizationService>();

            authorizationService.IsLoggedIn.Returns(true);
            authorizationService.IsAuthorized.Returns(true);

            var authorizationBehavior = new AuthorizationBehavior(authorizationService, outputWriter, innerBehavior);

            authorizationBehavior.Invoke();

            innerBehavior.Received().Invoke();
            outputWriter.DidNotReceiveWithAnyArgs().RedirectToUrl(null);
        }

        [Test]
        public void Should_Redirect_To_Login_Page_When_Not_Logged_In()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var outputWriter = Substitute.For<IOutputWriter>();
            var authorizationService = Substitute.For<IAuthorizationService>();

            authorizationService.IsLoggedIn.Returns(false);
            authorizationService.IsAuthorized.Returns(false);

            var authorizationBehavior = new AuthorizationBehavior(authorizationService, outputWriter, innerBehavior);

            authorizationBehavior.Invoke();

            innerBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl("/login");
        }

        [Test]
        public void Should_Redirect_To_Access_Denied_Page_When_Not_Authorized()
        {
            var innerBehavior = Substitute.For<IActionBehavior>();
            var outputWriter = Substitute.For<IOutputWriter>();
            var authorizationService = Substitute.For<IAuthorizationService>();

            authorizationService.IsLoggedIn.Returns(true);
            authorizationService.IsAuthorized.Returns(false);

            var authorizationBehavior = new AuthorizationBehavior(authorizationService, outputWriter, innerBehavior);

            authorizationBehavior.Invoke();

            innerBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl("/AccessDenied.htm");
        }
    }
}
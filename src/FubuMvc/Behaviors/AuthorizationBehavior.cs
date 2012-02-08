using Core.Domain;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace FubuMvc.Behaviors
{
    public class AuthorizationBehavior : IActionBehavior
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _innerBehavior;

        public AuthorizationBehavior(IAuthorizationService authorizationService, IOutputWriter writer, IActionBehavior innerBehavior)
        {
            _authorizationService = authorizationService;
            _writer = writer;
            _innerBehavior = innerBehavior;
        }

        public void Invoke()
        {
            if (!_authorizationService.IsLoggedIn) _writer.RedirectToUrl("/login");
            else if (!_authorizationService.IsAuthorized) _writer.RedirectToUrl("/AccessDenied.htm");
            else _innerBehavior.Invoke();
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}
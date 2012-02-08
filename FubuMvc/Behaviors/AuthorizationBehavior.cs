using Core.Infrastructure.Security;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace FubuMvc.Behaviors
{
    public class AuthorizationBehavior : IActionBehavior
    {
        private readonly ISecureSession _session;
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _innerBehavior;

        public AuthorizationBehavior(ISecureSession session, IOutputWriter writer, IActionBehavior innerBehavior)
        {
            _session = session;
            _writer = writer;
            _innerBehavior = innerBehavior;
        }

        public void Invoke()
        {
            if (_session.IsLoggedIn) _innerBehavior.Invoke();
            else _writer.RedirectToUrl("/AccessDenied.htm");
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}
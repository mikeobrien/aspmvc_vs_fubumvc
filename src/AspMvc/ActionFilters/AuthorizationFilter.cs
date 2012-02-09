using System.Web.Mvc;
using Core.Domain;

namespace AspMvc.ActionFilters
{
    public class AuthorizationFilter : IAuthorizationFilter, IMvcFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            Enabled = true;
        }

        public bool Enabled { get; set; }
        public bool AllowMultiple { get { return false; } }
        public int Order { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!Enabled) return;
            if (!_authorizationService.IsLoggedIn) {filterContext.Result = new RedirectResult("/login");}
            else if (!_authorizationService.IsAuthorized) filterContext.Result = new RedirectResult("/AccessDenied.htm");
        }
    }
}
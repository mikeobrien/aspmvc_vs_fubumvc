namespace Core.Domain
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool IsLoggedIn { get { return true; } }
        public bool IsAuthorized { get { return true; } }   
    }
}
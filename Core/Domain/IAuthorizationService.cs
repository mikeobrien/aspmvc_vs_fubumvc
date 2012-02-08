namespace Core.Domain
{
    public interface IAuthorizationService
    {
        bool IsLoggedIn { get; }
        bool IsAuthorized { get; }
    }
}
namespace Core.Infrastructure.Security
{
    public interface ISecureSession
    {
        bool IsLoggedIn { get; }     
    }
}
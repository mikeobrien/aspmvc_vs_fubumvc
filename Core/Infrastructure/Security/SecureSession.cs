namespace Core.Infrastructure.Security
{
    public class SecureSession : ISecureSession
    {
        public bool IsLoggedIn
        {
            get { return true; }
        }
    }
}
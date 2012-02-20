using System.Net;

namespace Core.Infrastructure.Web
{
    public interface IHttpStatus
    {
        void Set(int code, string description = null);
        void Set(HttpStatusCode code, string description = null);
    }
}
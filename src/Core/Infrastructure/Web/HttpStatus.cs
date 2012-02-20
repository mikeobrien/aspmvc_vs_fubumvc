using System.Net;
using System.Web;

namespace Core.Infrastructure.Web
{
    public class HttpStatus : IHttpStatus
    {
        public void Set(HttpStatusCode code, string description = null)
        {
            Set((int)code, description);
        }

        public void Set(int code, string description = null)
        {
            HttpContext.Current.Response.StatusCode = code;
            if (description != null) 
                HttpContext.Current.Response.StatusDescription = description;
        }
    }
}
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using FubuMVC.Core.Registration.DSL;

namespace Core.Infrastructure.Fubu
{
    public static class RouteConventionExpressionExtensions
    {
        public static RouteConventionExpression OverrideFolders(this RouteConventionExpression routeConvention)
        {
            RouteTable.Routes.Add(new IgnoreFilesRoute());
            RouteTable.Routes.RouteExistingFiles = true;
            return routeConvention;
        }

        public class IgnoreFilesRoute : Route
        {
            public IgnoreFilesRoute() : base(null, new StopRoutingHandler()) { }

            public override RouteData GetRouteData(HttpContextBase httpContext)
            {
                return HostingEnvironment.VirtualPathProvider.FileExists(httpContext.Request.AppRelativeCurrentExecutionFilePath) ?
                    new RouteData(this, RouteHandler) : null;
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary routeValues)
            {
                return null;
            }
        }
    }
}
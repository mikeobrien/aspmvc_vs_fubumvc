using System.Web.Routing;
using FubuMVC.Core.Registration.DSL;

namespace Core.Infrastructure.Fubu
{
    public static class RouteConventionExpressionExtensions
    {
         public static RouteConventionExpression HaveHigherPriorityThanFilesAndFolders(this RouteConventionExpression routeConvention)
         {
             RouteTable.Routes.RouteExistingFiles = true;
             return routeConvention;
         }
    }
}
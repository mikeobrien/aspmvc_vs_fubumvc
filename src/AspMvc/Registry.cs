using System.Web.Mvc;
using AspMvc.ActionFilters;
using Core.Domain;
using Core.Infrastructure.Data;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Web;

namespace AspMvc
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            ForSingletonOf<ILogger>().Use<Logger>();
            For<IExceptionFilter>().Use<ExceptionHandlerFilter>();

            For<IAuthorizationService>().Use<AuthorizationService>();
            For<IAuthorizationFilter>().Use<AuthorizationFilter>();

            For<IUnitOfWork>().Use<UnitOfWork>();
            ForSingletonOf<IRepository<DirectoryEntry>>().Use<Repository<DirectoryEntry>>();
            For<IActionFilter>().Use<TransactionScopeFilter>();
        }
    }
}
using System.Net.Mime;
using System.Web.Mvc;
using System.Web.Routing;
using AspMvc.Infrastructure;
using AspMvc.Models;
using AutoMapper;
using Core.Domain;
using StructureMap;

namespace AspMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ObjectFactory.Initialize(x => x.AddRegistry<Registry>());
            DependencyResolver.SetResolver(new StructureMapDependencyResolver());
            FilterProviders.Providers.Add(new FilterAttributeProvider(DependencyResolver.Current));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Mapper.CreateMap<DirectoryEntry, EntryModel>();
            Mapper.CreateMap<EntryModel, DirectoryEntry>();
        }

        // Hack to get the dependency resolver to dispose the nested container
        public override void Init()
        {
            ((StructureMapDependencyResolver)DependencyResolver.Current).RegisterHttpApplication(this);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Directory", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
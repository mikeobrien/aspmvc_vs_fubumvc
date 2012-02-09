using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspMvc.Infrastructure
{
    public abstract class DependencyResolverBase<T> : IDependencyResolver, IFilterProvider where T : class
    {
        private const string MetadataKey = "___DependencyResolverContainer___";

        private static readonly Type[] ActionFilterTypes = new[] 
                    { typeof(IAuthorizationFilter), typeof(IActionFilter), typeof(IResultFilter), 
                      typeof(IExceptionFilter), typeof(HandleErrorAttribute) };

        public abstract object GetInfrastructureService(Type serviceType);
        public abstract IEnumerable<object> GetInfrastructureServices(Type serviceType);

        public abstract T CreateActionContainer();
        public abstract object GetActionService(T container, Type serviceType);
        public abstract IEnumerable<object> GetActionServices(T container, Type serviceType);
        public abstract IEnumerable<object> GetActionFilters(T container, IEnumerable<Type> filterTypes);
        public abstract void ReleaseActionContainer(T container);

        public void ReleaseActionContainer()
        {
            var container = CurrentContainer;
            if (container != null) ReleaseActionContainer(container);
        }

        public void RegisterHttpApplication(HttpApplication application)
        {
            application.EndRequest += (s, e) => ReleaseActionContainer();
        }

        object IDependencyResolver.GetService(Type serviceType)
        {
            return IsInfrastructureService(serviceType) ?
                GetInfrastructureService(serviceType) :
                GetActionService(GetActionContainer(), serviceType);
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            return IsInfrastructureService(serviceType) ?
                GetInfrastructureServices(serviceType).
                    Union(GetBuiltInInfrastructureServices(serviceType)) :
                GetActionServices(GetActionContainer(), serviceType);
        }

        IEnumerable<Filter> IFilterProvider.GetFilters(ControllerContext controllerContext,
                                                       ActionDescriptor actionDescriptor)
        {
            return GetActionFilters(GetActionContainer(), ActionFilterTypes).
                        Select(x => new Filter(x, FilterScope.Global, null)).
                        Union(GetFilterAttributes(controllerContext, actionDescriptor));
        }

        private IEnumerable<Filter> GetFilterAttributes(ControllerContext controllerContext,
                                                        ActionDescriptor actionDescriptor)
        {
            return new FilterAttributeProvider(this).GetFilters(controllerContext, actionDescriptor);
        }

        private IEnumerable<object> GetBuiltInInfrastructureServices(Type serviceType)
        {
            return serviceType == typeof(IFilterProvider) ?
                                  new List<object> { this } :
                                  Enumerable.Empty<object>();
        }

        private T GetActionContainer()
        {
            var container = CurrentContainer;
            if (container == null) CurrentContainer = container = CreateActionContainer();
            return container;
        }

        private T CurrentContainer
        {
            get { return (T)HttpContext.Current.Items[MetadataKey]; }
            set { HttpContext.Current.Items[MetadataKey] = value; }
        }

        private static bool IsInfrastructureService(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith("System.Web.Mvc");
        }
    }
}
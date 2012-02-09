using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AspMvc.Infrastructure
{
    public class FilterAttributeProvider : IFilterProvider
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<FilterAttributeMetadata>> AttributeCache =
            new ConcurrentDictionary<string, IEnumerable<FilterAttributeMetadata>>();

        private readonly IDependencyResolver _dependencyResolver;

        public FilterAttributeProvider(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return GetFilterAttributes(actionDescriptor).Select(x => CreateFilter(x, _dependencyResolver));
        }

        private static IEnumerable<FilterAttributeMetadata> GetFilterAttributes(ActionDescriptor actionDescriptor)
        {
            IEnumerable<FilterAttributeMetadata> attributes;
            if (!AttributeCache.TryGetValue(actionDescriptor.UniqueId, out attributes))
            {
                attributes = LoadFilterAttributes(actionDescriptor);
                AttributeCache[actionDescriptor.UniqueId] = attributes;
            }
            return attributes;
        }

        private static IEnumerable<FilterAttributeMetadata> LoadFilterAttributes(ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes(typeof(IFilterAttribute), true).Cast<IFilterAttribute>().
                                    Select(x => new FilterAttributeMetadata(x, FilterScope.Action)).
                                    Union(actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(IFilterAttribute), true).
                                    Cast<IFilterAttribute>().
                                    Select(x => new FilterAttributeMetadata(x, FilterScope.Controller)));
        }

        private static Filter CreateFilter(FilterAttributeMetadata filterMetadata,
                                           IDependencyResolver dependencyResolver)
        {
            var filter = dependencyResolver.GetService(filterMetadata.Attribute.FilterType);
            filterMetadata.Attribute.InitializeFilter(filter);
            return new Filter(filter, filterMetadata.Scope, GetFilterOrder(filter));
        }

        private static int? GetFilterOrder(object filter)
        {
            var mvcFilter = filter as IMvcFilter;
            return mvcFilter != null ? mvcFilter.Order : (int?)null;
        }

        private class FilterAttributeMetadata
        {
            public FilterAttributeMetadata(IFilterAttribute attribute, FilterScope scope)
            { Attribute = attribute; Scope = scope; }

            public IFilterAttribute Attribute { get; private set; }
            public FilterScope Scope { get; private set; }
        }

        public interface IFilterAttribute
        {
            Type FilterType { get; }
            void InitializeFilter(object filter);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace AspMvc.Infrastructure
{
    public static class StructureMapExtensions
    {
        public static IEnumerable<T> GetAllInstances<T>(this IContainer container, Type type)
        {
            return container.GetAllInstances(type).Cast<T>();
        }

        public static IEnumerable<object> GetAllInstances(this IContainer container, IEnumerable<Type> types)
        {
            return container.Model.AllInstances.
                Join(types, x => x.PluginType, y => y, (x, y) => x).
                GroupBy(x => x.ConcreteType).
                Select(x => x.First().Get<object>());
        }
    }
}
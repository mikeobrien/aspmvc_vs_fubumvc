using System;
using System.Collections.Generic;
using StructureMap;

namespace AspMvc.Infrastructure
{
    public class StructureMapDependencyResolver : DependencyResolverBase<IContainer>
    {
        public override object GetInfrastructureService(Type serviceType)
        {
            return Resolve(ObjectFactory.Container, serviceType);
        }

        public override IEnumerable<object> GetInfrastructureServices(Type serviceType)
        {
            return ObjectFactory.Container.GetAllInstances<object>(serviceType);
        }

        public override IContainer CreateActionContainer()
        {
            return ObjectFactory.Container.GetNestedContainer();
        }

        public override object GetActionService(IContainer container, Type serviceType)
        {
            return Resolve(container, serviceType);
        }

        public override IEnumerable<object> GetActionServices(IContainer container, Type serviceType)
        {
            return container.GetAllInstances<object>(serviceType);
        }

        public override IEnumerable<object> GetActionFilters(IContainer container, IEnumerable<Type> filterTypes)
        {
            return container.GetAllInstances(filterTypes);
        }

        public override void ReleaseActionContainer(IContainer container)
        {
            container.Dispose();
        }

        private static object Resolve(IContainer container, Type serviceType)
        {
            return (serviceType.IsAbstract || serviceType.IsInterface) ?
                container.TryGetInstance(serviceType) :
                container.GetInstance(serviceType);
        }
    }
}
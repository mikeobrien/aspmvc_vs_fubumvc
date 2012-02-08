using Core.Domain;
using Core.Infrastructure.Data;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Security;

namespace FubuMvc
{
    public class Registry : StructureMap.Configuration.DSL.Registry
    {
        public Registry()
        {
            ForSingletonOf<ILogger>().Use<Logger>();

            For<ISecureSession>().Use<SecureSession>();

            For<IUnitOfWork>().Use<UnitOfWork>();
            ForSingletonOf<IRepository<DirectoryEntry>>().Use<Repository<DirectoryEntry>>();
        }
    }
}
using System;

namespace Core.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITransaction BeginTransaction();
    }
}

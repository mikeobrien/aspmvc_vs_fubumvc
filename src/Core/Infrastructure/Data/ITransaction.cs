using System;

namespace Core.Infrastructure.Data
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}

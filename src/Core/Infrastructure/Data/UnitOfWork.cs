namespace Core.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITransaction BeginTransaction()
        {
            return new Transaction();
        }

        public void Dispose() { }
    }
}
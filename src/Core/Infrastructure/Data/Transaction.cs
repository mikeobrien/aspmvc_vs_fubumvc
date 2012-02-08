namespace Core.Infrastructure.Data
{
    public class Transaction : ITransaction
    {
        public void Commit() { }
        public void Rollback() { }
        public void Dispose() { }
    }
}
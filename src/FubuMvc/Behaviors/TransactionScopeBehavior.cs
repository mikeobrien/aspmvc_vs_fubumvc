using Core.Infrastructure.Data;
using FubuMVC.Core.Behaviors;

namespace FubuMvc.Behaviors
{
    public class TransactionScopeBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionScopeBehavior(IActionBehavior innerBehavior, IUnitOfWork unitOfWork)
        {
            _innerBehavior = innerBehavior;
            _unitOfWork = unitOfWork;
        }

        public void Invoke()
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _innerBehavior.Invoke();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}

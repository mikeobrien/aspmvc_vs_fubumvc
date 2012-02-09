using System.Web.Mvc;
using Core.Infrastructure.Data;

namespace AspMvc.ActionFilters
{
    // Had to implement IAuthorizationFilter as a hack to start the 
    // transaction before the authorization filter fired. :(
    public class TransactionScopeFilter : IAuthorizationFilter, IActionFilter, IMvcFilter
    {
        private readonly IUnitOfWork _unitOfWork;
        private ITransaction _transaction;

        public TransactionScopeFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Enabled = true;
        }

        public bool Enabled { get; set; }
        public bool AllowMultiple { get { return false; } }
        public int Order { get { return 0; } }


        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!Enabled) return;
            _transaction = _unitOfWork.BeginTransaction();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!Enabled) return;
            try
            {
                if (filterContext.Exception == null) _transaction.Commit();
                else _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
            }
        }
    }
}
using System;
using AspMvc.Infrastructure;

namespace AspMvc.ActionFilters
{
    public class OverrideTransactionScopeAttribute : Attribute, FilterAttributeProvider.IFilterAttribute
    {
        public Type FilterType { get { return typeof(TransactionScopeFilter); } }

        public void InitializeFilter(object filter)
        {
            ((TransactionScopeFilter) filter).Enabled = false;
        }
    }
}
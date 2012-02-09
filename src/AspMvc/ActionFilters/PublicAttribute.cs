using System;
using AspMvc.Infrastructure;

namespace AspMvc.ActionFilters
{
    public class PublicAttribute : Attribute, FilterAttributeProvider.IFilterAttribute
    {
        public Type FilterType { get { return typeof(AuthorizationFilter); } }

        public void InitializeFilter(object filter)
        {
            ((AuthorizationFilter) filter).Enabled = false;
        }
    }
}
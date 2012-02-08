using System.Linq;
using FubuMVC.Core.Registration.DSL;

namespace Core.Infrastructure.Fubu
{
    public static class ConfigurationExtensions
    {
        public static ActionCallCandidateExpression IncludeTypeNamesSuffixed(this ActionCallCandidateExpression expression, params string[] suffix)
        {
            suffix.ToList().ForEach(x => expression.IncludeTypes(y => y.Name.EndsWith(x)));
            return expression;
        }

        public static ActionCallCandidateExpression IncludeMethodsPrefixed(this ActionCallCandidateExpression expression, params string[] prefix)
        {
            prefix.ToList().ForEach(x => expression.IncludeMethods(y => y.Name.StartsWith(x)));
            return expression;
        }

        public static RouteConventionExpression ConstrainMethodPrefixToHttpGet(this RouteConventionExpression expression, string prefix)
        {
            return expression.ConstrainToHttpMethod(x => x.Method.Name.StartsWith(prefix), "GET");
        }

        public static RouteConventionExpression ConstrainMethodPrefixToHttpPost(this RouteConventionExpression expression, string prefix)
        {
            return expression.ConstrainToHttpMethod(x => x.Method.Name.StartsWith(prefix), "POST");
        }

        public static RouteConventionExpression ConstrainMethodPrefixToHttpPut(this RouteConventionExpression expression, string prefix)
        {
            return expression.ConstrainToHttpMethod(x => x.Method.Name.StartsWith(prefix), "PUT");
        }

        public static RouteConventionExpression ConstrainMethodPrefixToHttpDelete(this RouteConventionExpression expression, string prefix)
        {
            return expression.ConstrainToHttpMethod(x => x.Method.Name.StartsWith(prefix), "DELETE");
        }
    }
}

using Core.Infrastructure.Fubu;
using FubuMVC.Core;
using FubuMVC.Spark;
using FubuMvc.Behaviors;
using FubuMvc.Directory;

namespace FubuMvc
{
    public class Conventions : FubuRegistry
    {
        public Conventions()
        {
            Actions
                .IncludeTypeNamesSuffixed("Handler")
                .IncludeMethodsPrefixed("Execute");

            Routes
                .OverrideFolders()
                .HomeIs<IndexPublicGetHandler>(x => x.Execute(null))
                .UrlPolicy(RegexUrlPolicy.Create()
                    .IgnoreAssemblyNamespace()
                    .IgnoreClassName()
                    .IgnoreMethodNames("Execute")
                    .ConstrainClassToHttpGetEndingWith("GetHandler")
                    .ConstrainClassToHttpGetEndingWith("PublicGetHandler")
                    .ConstrainClassToHttpPostEndingWith("PostHandler")
                    .ConstrainClassToHttpPostEndingWith("PublicPostHandler")
                    .ConstrainClassToHttpPutEndingWith("PutHandler")
                    .ConstrainClassToHttpPutEndingWith("PublicPutHandler")
                    .ConstrainClassToHttpDeleteEndingWith("DeleteHandler")
                    .ConstrainClassToHttpDeleteEndingWith("PublicDeleteHandler"));

            Media.ApplyContentNegotiationToActions(x => x.HandlerType.Assembly == GetType().Assembly && !x.HasAnyOutputBehavior());

            Policies
                .ConditionallyWrapBehaviorChainsWith<HtmlOutputBehavior>(x => x.Method.ReturnType == typeof(string))
                .ConditionallyWrapBehaviorChainsWith<AuthorizationBehavior>(x => !x.Method.DeclaringType.Name.Contains("Public"))
                .ConditionallyWrapBehaviorChainsWith<TransactionScopeBehavior>(x => !x.HasAttribute<OverrideTransactionScopeAttribute>())
                .ConditionallyWrapBehaviorChainsWith<AjaxExceptionHandlerBehavior>(x => !x.HasAnyOutputBehavior())
                .ConditionallyWrapBehaviorChainsWith<ExceptionHandlerBehavior>(x => x.HasAnyOutputBehavior());

            this.UseSpark();

            Views.TryToAttachWithDefaultConventions();
        }
    }
}
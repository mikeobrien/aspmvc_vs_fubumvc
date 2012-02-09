using System.Web.Routing;
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
                .HaveHigherPriorityThanFilesAndFolders()
                .HomeIs<PublicGetHandler>(x => x.Execute(null))
                .UrlPolicy(RegexUrlPolicy.Create()
                    .IgnoreAssemblyNamespace(GetType(), "Handlers")
                    .IgnoreClassName()
                    .IgnoreMethodNames("Execute")
                    .ConstrainClassToHttpGetStartingWith("Get")
                    .ConstrainClassToHttpGetStartingWith("PublicGet")
                    .ConstrainClassToHttpPostStartingWith("Post")
                    .ConstrainClassToHttpPostStartingWith("PublicPost")
                    .ConstrainClassToHttpPutStartingWith("Put")
                    .ConstrainClassToHttpPutStartingWith("PublicPut")
                    .ConstrainClassToHttpDeleteStartingWith("Delete")
                    .ConstrainClassToHttpDeleteStartingWith("PublicDelete"));

            Media.ApplyContentNegotiationToActions(x => x.HandlerType.Assembly == GetType().Assembly && !x.HasAnyOutputBehavior());

            Policies
                .ConditionallyWrapBehaviorChainsWith<AuthorizationBehavior>(x => !x.Method.DeclaringType.Name.StartsWith("Public"))
                .ConditionallyWrapBehaviorChainsWith<TransactionScopeBehavior>(x => !x.HasAttribute<OverrideTransactionScopeAttribute>())
                .WrapBehaviorChainsWith<ExceptionHandlerBehavior>();

            this.UseSpark();

            Views.TryToAttachWithDefaultConventions();
        }
    }
}
using FubuCore;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Urls;

namespace Core.Infrastructure.Fubu
{
    public static class UrlRegistryExtensions
    {
        public static string QuotedUrlFor<TInputModel>(this IUrlRegistry urls)
        {
            return "'{0}'".ToFormat(urls.UrlFor(typeof(TInputModel), new RouteParameters()));
        }
    }
}
using System.Web;
using FubuMVC.Core.Behaviors;

namespace FubuMvc.Behaviors
{
public class HtmlOutputBehavior : IActionBehavior
{
    private readonly IActionBehavior _innerBehavior;

    public HtmlOutputBehavior(IActionBehavior innerBehavior)
    {
        _innerBehavior = innerBehavior;
    }

    public void Invoke()
    {
        _innerBehavior.Invoke();
        HttpContext.Current.Response.ContentType = "text/html";
    }

    public void InvokePartial()
    {
        _innerBehavior.InvokePartial();
    }
}
}

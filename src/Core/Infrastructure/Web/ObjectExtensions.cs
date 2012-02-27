using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Infrastructure.Web
{
    public static class ObjectExtensions
    {
        public static string ToJavaScript(this object source)
        {
            return JsonConvert.SerializeObject(source, new JavaScriptDateTimeConverter());
        }
    }
}
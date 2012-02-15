using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Core.Infrastructure.Reflection
{
    public static class AssemblyExtensions
    {
        public static bool IsInDebugMode(this Assembly assembly)
        {
            return assembly.GetCustomAttributes(typeof(DebuggableAttribute), false)
                .Cast<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);
        }
    }
}
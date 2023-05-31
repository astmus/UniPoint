using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace MissCore.Extensions
{
    public static class CommonExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string value)
                => string.IsNullOrEmpty(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullOrEmpty(this string value)
            => !string.IsNullOrEmpty(value);
        public static string GetTypeIdentifier(this object value)
            => value.GetType().ToString();
        public static void WriteError(this ILogger log, Exception e, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = default)
            => log.LogError(e ?? new Exception("Write to log exceptio"), $" {name} {line} {path} ");
        public static void WriteCritical(this ILogger log, Exception e, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = default)
            => log.LogCritical(e, $" {name} {line} {path} ");
        public static void Write(this ILogger log, string val, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = default)
            => log.LogError($" {path} \t{name} {line}  {val}");

    }

}


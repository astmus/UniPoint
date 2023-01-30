using Microsoft.Extensions.Logging;
using MissCore.Configuration;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace BotService.Common
{
    internal static class CommonExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string value)
                => string.IsNullOrEmpty(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullOrEmpty(this string value)
            => !string.IsNullOrEmpty(value);
        public static string GetTypeIdentifier(this object value)
            => value.GetType().ToString();
        public static void Write(this ILogger log, Exception e, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = default)
            => log.LogError(e ?? new Exception("Write to log exceptio"), $" {name} {line} {path} ");
        public static void WriteCritical(this ILogger log, Exception e, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = default)
            => log.LogCritical(e, $" {name} {line} {path} ");
        public static void Write(this ILogger log, string val, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = default)
            => log.LogError($" {path} \t{name} {line}  {val}");
        public static void WriteJson(this ILogger log, object val)
           => log.LogInformation(JsonConvert.SerializeObject(val, Formatting.Indented, DataTransformExtension.ContentSerializerSettings));
        public static void Write(this ILogger log, string information)
            => log.LogInformation(information);
        //	public static Task SendMessageNotification(this IMediator mediator, string message, CancellationToken cancel = default)
        //		=> mediator.Send(new MessageNotifyRequest(message), cancel);
        //	public static Task SendExceptionNotifyRequest(this IMediator mediator, Exception error, CancellationToken cancel = default)
        //		=> mediator.Send(new ExceptionNotifyRequest(error), cancel);
        //}
    }
}


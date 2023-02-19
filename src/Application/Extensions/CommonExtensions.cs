using System.Reflection;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Configuration;

namespace MissBot.Extensions
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
        //	public static Task SendMessageNotification(this IMediator mediator, string message, CancellationToken cancel = default)
        //		=> mediator.Send(new MessageNotifyRequest(message), cancel);
        //	public static Task SendExceptionNotifyRequest(this IMediator mediator, Exception error, CancellationToken cancel = default)
        //		=> mediator.Send(new ExceptionNotifyRequest(error), cancel);
        //}
    }
    public static class BotBuilderExtension
    {
        public static IBotBuilder<TBot> UseMediator<TBot>(this IBotBuilder<TBot> botBuilder) where TBot : class, IBot
        {
            botBuilder.BotServices.AddMediatR(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly());
            return botBuilder;
        }
        public static IBotBuilder<TBot> UseLogging<TBot>(this IBotBuilder<TBot> botBuilder) where TBot : class, IBot
        {
            botBuilder.BotServices.AddLogging();
            return botBuilder;
        }        
    }
}


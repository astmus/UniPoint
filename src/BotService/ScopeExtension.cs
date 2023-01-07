using System.Reflection;
using MissBot.Attributes;
using MissBot.Interfaces;
using MissCore.Abstractions;

namespace BotService
{
    internal static class BotCommandExtension
    {
        public static IEnumerable<IBotCommandInfo> GetAttributedCommands<TBot>(this Type botType) where TBot : class, IBot
        => botType.GetCustomAttributes<HasBotCommandAttribute>().Select(s => s.Command());

        public static IEnumerable<IBotCommandInfo> GetDefinedCommands<TBot>(this TBot bot) where TBot : class, IBot
            => bot.GetType().GetAttributedCommands<TBot>();
    }
}

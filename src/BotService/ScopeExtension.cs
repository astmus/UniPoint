using System.Reflection;
using MissBot.Abstractions;
using MissBot.Attributes;
using MissCore.Configuration;

namespace BotService
{
    public static class BotCommandExtension
    { 
        record CommandData(string Command, Type CmdType, string Description) : IBotCommandInfo;
        public static IEnumerable<HasBotCommandAttribute> GetCommandAttributes<TBot>(this Type botType) where TBot : class, IBot
            => botType.GetCustomAttributes<HasBotCommandAttribute>();      
        public static IEnumerable<IBotCommandInfo> GetCommandsFromAttributes(this object bot)
            => bot.GetType().GetCustomAttributes<HasBotCommandAttribute>().Select(s => new CommandData($"/{s.Name.ToLower()}", s.CmdType, s.Description));
    }
   
}

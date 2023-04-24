using System.Reflection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Attributes;

namespace BotService
{
    public static class BotCommandExtension
    {
        record CommandData(string CommandAction, Type CmdType, string Description) : IBotCommand
        {
            public string Command { get; set; }            
        }

        public static IEnumerable<HasBotCommandAttribute> GetCommandAttributes<TBot>(this Type botType) where TBot : class, IBot
            => botType.GetCustomAttributes<HasBotCommandAttribute>();      
        public static IEnumerable<IBotCommand> GetCommandsFromAttributes(this object bot)
            => bot.GetType().GetCustomAttributes<HasBotCommandAttribute>().Select(s => new CommandData($"/{s.Name.ToLower()}", s.CmdType, s.Description));
    }
   
}

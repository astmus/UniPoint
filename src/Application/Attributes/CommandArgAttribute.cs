using MissBot.Common;
using MissBot.Interfaces;

namespace MissBot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class HasBotCommandAttribute : Attribute
    {
        public string Name { get; init; }
        public string Description { get; set; }

        public static implicit operator MissCommand<Telegram.Bot.Types.BotCommand>(HasBotCommandAttribute attr)
            => new MissCommand<Telegram.Bot.Types.BotCommand>() { Command = $"/{attr.Name.ToLower()}", Description = attr.Description };

        public IBotCommandInfo Command()
            => new MissCommand<Telegram.Bot.Types.BotCommand>() { Command = $"/{Name.ToLower()}", Description = Description };
    }
}

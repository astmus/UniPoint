using MissBot.Common;
using Telegram.Bot.Types;

namespace MissBot.Interfaces
{
    public interface IBotCommandData
    {
        string Payload { get; init; }
        string[] Params { get; init; }
    }

    public interface IBotCommandInfo
    {
        virtual MissCommand<BotCommand> CommandData => default!;
        string Command { get; }
        string Description { get; }
    }
}

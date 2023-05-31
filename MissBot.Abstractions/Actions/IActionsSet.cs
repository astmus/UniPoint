using Telegram.Bot.Types;

namespace MissBot.Abstractions.Actions;

public interface IActionsSet
{
}

public interface IChatActionsSet : IActionsSet
{
    public IActionsSet RemoveKeyboard();
}

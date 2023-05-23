using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Entities.Query;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class CallbackQuery<TUnit> : CallbackQuery
{
    
}

using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;

namespace MissCore.Bot
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BotEntityAction<TEntity> : BotUnitCommand, IUnitAction<BotCommand> where TEntity : class
    {
        public string[] Params { get; set; }
    }
}

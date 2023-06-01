using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;

namespace MissCore.Bot
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotUnitAction<TEntity> : BotUnitAction, IBotUnitAction<TEntity> where TEntity : class
    {
        public string[] Params { get; set; }
        public Id<TEntity> UnitIdentifier { get; set; }
    }
}

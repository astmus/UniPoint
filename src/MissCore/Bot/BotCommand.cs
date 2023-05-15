using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;

namespace MissCore.Bot
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BotEntityAction<TEntity> : BotAction, IBotAction<BotCommand> where TEntity : class
    {
        public string[] Params { get; set; }
        public string Placeholder { get; set; }
        public string Payload { get; set; }
    }
}

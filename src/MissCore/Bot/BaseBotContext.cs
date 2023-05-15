namespace MissCore.Bot
{

    public record BotUnit2<TEntity>(TEntity Unit = default) : BotUnit
    {
        public static readonly string EntityName = typeof(TEntity).Name;
        [JsonProperty]
        public override string Entity
            => EntityName;

    }
}

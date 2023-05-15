namespace MissCore.Bot
{
    public abstract record BotEntity
    {
        [JsonProperty]
        public abstract string Entity { get; }
    }
}

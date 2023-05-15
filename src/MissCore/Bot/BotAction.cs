namespace MissCore.Bot
{
    public abstract record BotAction : BotEntity
    {
        public virtual string Command { get; set; }
        public abstract string CommandAction { get; }
    }
}

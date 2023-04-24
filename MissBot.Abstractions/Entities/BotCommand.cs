namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand : BotAction, IBotCommand
    {
        /// <summary>
        /// Text of the command, 1-32 characters. Can contain only lowercase English letters, digits and underscores.
        /// </summary>  
        [JsonProperty("command")]
        public override string Command { get; set; }

        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonIgnore]
        public override string Entity
            => nameof(BotCommand);

        public override string CommandAction
            => Command;
    }

    public class BotCommandList : List<BotCommand>
    { }

    public static class BotCommand<TCommand> where TCommand : BotCommand
    {
        public static readonly string Name = typeof(TCommand).Name.ToLower();
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public record BotCommandUnit : BotCommand, IBotUnit
    {
        [JsonProperty]
        public override string Entity
            => nameof(BotCommand);
        public string[] Params { get; set; }
        public string Payload { get; set; }
        public string Placeholder { get; set; }
        public override string CommandAction
               => Command;
    }
}

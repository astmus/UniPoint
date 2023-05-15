namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand : IBotCommand, IBotUnit
    {
        /// <summary>
        /// Text of the command, 1-32 characters. Can contain only lowercase English letters, digits and underscores.
        /// </summary>  
        [JsonProperty("command")]
        public  string Command { get; set; }

        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonIgnore]
        public virtual  string Entity
            => nameof(BotCommand);

        public virtual  string CommandAction
            => Command;

        public string Payload { get; set; }
        public string Placeholder { get; set; }
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

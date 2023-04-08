using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

namespace MissBot.Common
{
    
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record CommandData : IBotCommandInfo, IBotCommandData
    {
        /// <summary>
        /// Text of the command, 1-32 characters. Can contain only lowercase English letters, digits and underscores.
        /// </summary>

        [JsonProperty(Required = Required.Always)]
        public string Command { get; set; } = default!;
        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; } = default!;

        public string CustomParams { get; set; }
        public string Payload { get; set; }

        public string[] Params { get; set; }

        public Type CmdType => this.CmdType;

        public static implicit operator CommandData(string cmd)
            => new CommandData($"/{cmd}".Replace("//", "/").ToLower());
        public static implicit operator string(CommandData cmd)
            => $"/{cmd.Command}".Replace("//", "/").ToLower();

        public static bool operator ==(string commandA, CommandData commandB)
        => commandA?.EndsWith(commandB?.Command?.ToLower()) == true ||
                    commandB?.Command.ToLower().EndsWith(commandA ?? string.Empty) == true;

        public static bool operator !=(string commandA, CommandData commandB)
            => commandA?.EndsWith(commandB?.Command?.ToLower()) == false &&
                 commandB?.Command?.ToLower().EndsWith(commandA ?? string.Empty) == false;

    }



}

using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

namespace MissBot.Common
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommandInfo : IBotCommandInfo
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

        [JsonIgnore]
        public MissCommand<BotCommand> CommandData { get; set; }

        public Type CmdType => ((IBotCommandInfo)CommandData).CmdType;

        public static implicit operator BotCommandInfo(string cmd)
            => new BotCommandInfo($"/{cmd}".Replace("//", "/").ToLower());
        public static implicit operator string(BotCommandInfo cmd)
            => $"/{cmd.Command}".Replace("//", "/").ToLower();

        public static bool operator ==(string commandA, BotCommandInfo commandB)
        => commandA?.EndsWith(commandB?.Command?.ToLower()) == true ||
                    commandB?.Command.ToLower().EndsWith(commandA ?? string.Empty) == true;

        public static bool operator !=(string commandA, BotCommandInfo commandB)
            => commandA?.EndsWith(commandB?.Command?.ToLower()) == false &&
                 commandB?.Command?.ToLower().EndsWith(commandA ?? string.Empty) == false;

    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record MissCommand<TInfoData> : BotCommandInfo, IBotCommandData where TInfoData : BotCommand
    {
        public string CustomParams { get; set; }
        public string Payload { get; set; }
        public string Name { get; set; }
        string[] IBotCommandData.Params { get; set; }

        public record Cmd(string name);
        public record Params(params string[] args);
    }


}

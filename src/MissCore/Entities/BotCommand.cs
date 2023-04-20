using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

namespace MissCore.Entities
{


    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class BotCommand<TResult> : BotCommand, IBotCommand
    {
        [JsonProperty("Command" , Required = Required.AllowNull)]
        public abstract string EntityAction { get; } 
        public string Payload { get; set; }
        public string[] Params { get; set; }
    }
    public class ActionPayload
    {
        public virtual string EntityAction { get; set; }
        public string Payload { get; set; }
        public string[] Params { get; set; }

    }

}

using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace MissCore.Entities
{
    

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class BotCommand<TResult> : BotAction, IBotCommand
    {
        [JsonProperty("command", Required = Required.AllowNull)]
        public override abstract string Command { get;  } 
    }
    public class BotAction : BotCommand
    {
        public virtual string Entity { get; set; }
        public virtual string EntityAction { get;  }
        public string Payload { get; set; }
        public string Placeholder { get; set; }
        public string[] Params { get; set; }

    }

}

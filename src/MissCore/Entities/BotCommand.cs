using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

namespace MissCore.Entities
{

    
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class BotCommand<TResult> : BotCommand, IBotCommandData, IBotCommandInfo
    {        
        public string Payload { get; set; }
        public string[] Params { get; set; }

    }

}

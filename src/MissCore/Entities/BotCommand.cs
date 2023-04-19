using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

namespace MissCore.Entities
{


    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class BotCommand<TResult> : BotCommand, IBotCommand
    {
        public abstract string CommandName { get; }
        public string Payload { get; set; }
        public string[] Params { get; set; }
    }
    public class ActionPayload
    {
        public string Payload { get; set; }
        public string[] Params { get; set; }

    }

}

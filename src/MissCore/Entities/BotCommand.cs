using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace MissCore.Entities
{


    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BotEntityAction<TEntity> : BotAction<TEntity>, IBotAction<BotCommand> where TEntity:class
    {
        public string[] Params { get; set; }
        public string Placeholder { get; set; }
        public string Payload { get; set; }
    }

  

}

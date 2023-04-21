using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace MissCore.Entities
{


    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BotCommand<TOfEntity> : BotAction, IBotAction, IBotCommand, IEntityAction<TOfEntity> where TOfEntity:class
    {
        public string[] Params { get; set; }
    }

  

}

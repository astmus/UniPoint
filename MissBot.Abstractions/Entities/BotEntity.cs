using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions.Actions;

namespace MissBot.Abstractions.Entities
{    
    public abstract record BotEntity
    {
        [JsonProperty]
        public abstract string Entity { get; }
    }
    public abstract record BotAction : BotEntity 
    {
        public virtual string Command { get; set; }
        public abstract string CommandAction { get; }        
    }

    public record BotUnit : Unit, IBotUnit
    {
        //public string Entity { get; set; }        
        public string Command { get; set; }
        public string Placeholder { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
    }

    public abstract record BotUnit<TEntity>(TEntity Unit = default) : BotUnit
    {
        public static readonly string EntityName = typeof(TEntity).Name;
        [JsonProperty]
        public string Entity
            => EntityName;

    }
}

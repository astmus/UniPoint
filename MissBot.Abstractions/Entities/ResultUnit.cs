
namespace MissBot.Abstractions.Entities
{          
    public abstract record ResultUnit<T> : ResultUnit
    {        
        public abstract T Content { get; set; }     
    }
    public abstract record ResultUnit : BaseUnit
    {
        public abstract string Id { get; set; }
        public abstract string Title { get; set; }
        public abstract string Description { get; set; }
    }
    
    }

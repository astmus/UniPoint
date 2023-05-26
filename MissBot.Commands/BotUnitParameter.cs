namespace MissBot.Entities
{    
    public abstract record BotUnitParameter
    {    
        public virtual string Unit { get; set; }
        public virtual string Name { get; set; }
        public virtual string Template { get; set; }
        public virtual string Value { get; set; }
    }
}

namespace MissBot.Entities
{    
    public abstract record BotUnitParameter
    {    
        public virtual string Unit { get; set; }
        public virtual string Name { get; set; }
        public virtual string Template { get; set; }
        public virtual string Value { get; set; }
    }
 
    public static class UnitRequestParameter
    {
        public static UnitRequestParameter<TName, TValue> Create<TName, TValue>(ref TName name, TValue value)
             => new UnitRequestParameter<TName, TValue>(name, value);
    }
    public readonly record struct UnitRequestParameter<TName, TValue>(TName Name, TValue Value)
    {
    }
   
}

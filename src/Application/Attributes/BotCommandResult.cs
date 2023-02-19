namespace MissBot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class BotCommandResultAttribute : Attribute
    {
        public string Name { get; init; }
        public object Value { get; set; }        
        //public IBotCommandData Command { get; set; }
    }
}

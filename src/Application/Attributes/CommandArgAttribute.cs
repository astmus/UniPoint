namespace MissBot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class HasBotCommandAttribute : Attribute
    {
        public string Name { get; init; }
        public Type CmdType { get; init; }
        public string Description { get; set; }        
    }
}

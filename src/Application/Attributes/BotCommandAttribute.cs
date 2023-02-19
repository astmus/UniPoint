namespace MissBot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BotCmdArgAttribute : Attribute
    {
        public string Name { get; init; }
        public object Value { get; set; }
        public bool Optional { get; init; }
        //public IBotCommandData Command { get; set; }
    }
}

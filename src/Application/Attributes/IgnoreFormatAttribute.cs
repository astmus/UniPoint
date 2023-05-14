namespace MissBot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IgnoreFormatAttribute : Attribute
    {
    }
}

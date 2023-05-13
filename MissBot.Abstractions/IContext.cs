namespace MissBot.Abstractions
{

    public interface IContext
    {
        object this[string index] { get; }
        T Get<T>(Predicate<string> filter = null);
        T Get<T>();
        T Get<T>(string name);
        TAny Any<TAny>();
        T Set<T>(T value, string name = null);
    }
}

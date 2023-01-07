namespace MissCore.Abstractions
{

    public interface IContext
    {
        T Get<T>(Predicate<string> filter = null);
        T Get<T>();
        T Set<T>(T value, string name = null);
        T Get<T>(string name);
    }
}

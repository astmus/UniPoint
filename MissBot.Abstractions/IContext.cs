using System.Runtime.CompilerServices;

namespace MissBot.Abstractions
{

    public interface IContext
    {
        object this[string index] { get; }
        T Get<T>(Predicate<string> filter);
        //T Get<T>();
        T Get<T>([CallerMemberName] string name = default);
        TAny Any<TAny>();
        T Set<T>(T value, string name = null);
    }
}

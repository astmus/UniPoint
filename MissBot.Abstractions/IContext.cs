using System.Runtime.CompilerServices;

namespace MissBot.Abstractions
{               
    public interface IContext
    {        
        object this[string index] { get; }
        public T TakeByKey<T>();
        T Take<T>([CallerMemberName] string name = default);
        T Get<T>();
        TAny Any<TAny>();
        T Set<T>(T value, string name = null);        
    }
}

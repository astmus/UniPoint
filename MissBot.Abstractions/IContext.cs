using System.Runtime.CompilerServices;
using MissBot.Abstractions.DataAccess;

namespace MissBot.Abstractions
{
    public interface IContext
    {        
        object this[string index] { get; }
        //T TakeByKey<T>();
        T Take<T>([CallerMemberName] string name = default);
        T Take<T>(Id<T> identifier);
        T Get<T>();
        TAny Any<TAny>();
        T Set<T>(T value, string name = null);        
    }
}

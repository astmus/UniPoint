using MissBot.Abstractions.Utils;

namespace MissBot.Abstractions.DataContext
{
    public record Id<T>(string id) : Id(id)
    {
        public static readonly Id<T> Value = new Id<T>(typeof(T).Name.ToSnakeCase());
    }
    public record Id<T, T2>(string id) : Id<T>(id)
    {
        public static new readonly Id<T, T2> Value = new Id<T,T2>($"{typeof(T).Name}.{typeof(T2).Name} ".ToSnakeCase());
    }
    public record Id(string id)
    {
        public static implicit operator string(Id id)
            => id.id;        
    }
}

using MissBot.Abstractions.Utils;

namespace MissBot.Abstractions.DataAccess
{
    public record Id<T>(string id) : Id(id)
    {
        public static readonly Id<T> Value = new Id<T>(typeof(T).Name);        
        public Id<T> Add(string add) => Value with { id = id + add };
        public Id<T> With(string id) => Value with { id = id };
    }
    public record Id<T, T2>(string unitId, string entityId) : Id<T>(unitId+entityId)
    {
        public static new readonly Id<T, T2> Value = new Id<T, T2>($"{typeof(T).Name}", typeof(T2).Name);
    }
    public record Id(string id)
    {
        public static implicit operator string(Id id)
            => id.id;
        public static explicit operator Id(string id)
            => new Id(id);
    }
}

using MissBot.Abstractions.Utils;

namespace MissBot.Abstractions.DataAccess
{
    public record Identifier(object value)
    {
        public static implicit operator string(Identifier id)
            => $"{(id.value as Type).Name.Replace("'", "[]")}: {id.GetHashCode()}";
    }
    public record Id<T>(string id) : Id(id)
    {
        public static readonly Id<T> Value = new Id<T>(typeof(T).Name.ToSnakeCase());
    }

    public record Id(string id)
    {
        public static implicit operator string(Id id)
            => id.id;

        public static Id<T> Of<T>(T data = default)
            => Id<T>.Value;
        public static string Of(object data)
            => new Identifier(data);
    }

    public class Identifier<T>
    {
        protected Identifier(object data = null)
        {

            instance = this;
        }

        static Identifier<T> instance;

        public static Identifier<T> Current
            => instance ?? new Identifier<T>();
        private Id<T> Id;

        public static Id<T> TypeId
            => Current.Id;




    }

}

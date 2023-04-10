namespace MissCore.Data.Identity
{
    public record Identifier(object value)
    {
        public static implicit operator string(Identifier id)
            => $"{(id.value as Type).Name.Replace("'", "[]")}: {id.GetHashCode()}";
    }
    public record Id<T>(object value = null) : Identifier(value ?? typeof(T))
    {
        public Type IdentityType { get; protected set; }
        public static explicit operator string(Id<T> id)
            => $"{(id.value as Type).Name.Replace("'", "[]")}: {id.GetHashCode()}";
        public static implicit operator Type(Id<T> id)
            => id.value as Type;
        public static implicit operator long(Id<T> id)
            => Convert.ToInt64(id.value?.GetHashCode() ?? typeof(T).GUID.GetHashCode());
    }

    public static class Identity
    {
        public static Id<T> Of<T>(T data = default)
            => Identifier<T>.Current.Next;
        public static string Of(object data)
            => new Identifier(data);
    }

    public class Identifier<T>
    {
        protected Identifier(object data = null)
        {
            Id = new Id<T>(data?.GetType());
            instance = this;
        }

        static Identifier<T> instance;
        public Id<T> Next
            => Id with { value = Id + 1 };
        public static Identifier<T> Current
            => instance ?? new Identifier<T>();
        private Id<T> Id;
        public static Type Type
            => Current.Id;
        public static string TypeId
            => (string)Current.Id;

        public Id<T> this[T data]
            => new Id<T>(data);


    }

}

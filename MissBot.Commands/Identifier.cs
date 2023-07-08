
using System.Globalization;

namespace MissBot.Identity
{
	public record Id<T>(string Key) : Id(Key)
	{
		public static readonly Id<T> Instance;

		static Id()
			=> Instance = new Id<T>(typeof(T).Name.Replace("`1", string.Empty));

		public Id<T> Combine(object appendKey) => Instance with { Key = Key + appendKey };
		public Id<T> Combine(params object[] ids) => Instance with { Key = string.Join('.', ids) };
		public Id<T> With(string key) => Instance with { Key = key };

		public static Id<T> Join(params object[] ids)
			=> Instance.With(string.Join('.', ids));
	}

	public record Id<T, T2>(string unitId, string entityId) : Id<T>(unitId + "." + entityId)
	{
		public static readonly Id<T, T2> Value = new Id<T, T2>(typeof(T).Name, typeof(T2).Name);
	}

	public record Id(string Key)
	{

		public static implicit operator string(Id id)
			=> id.Key;

		public static implicit operator long(Id id)
			=> long.Parse(id.Key, CultureInfo.CurrentCulture);

		public static implicit operator int(Id id)
			=> int.Parse(id.Key, CultureInfo.CurrentCulture);

		public static implicit operator Id(int id)
			=> new Id($"{id}");

		public static implicit operator Id(long id)
			=> new Id($"{id}");
	}
}

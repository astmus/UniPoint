
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace MissBot.Identity
{
	public record Id<T>(string Value) : Id(Value)
	{
		public static readonly Id<T> Instance;
		public static Type InnerType { get; }
		static Id()
		{
			InnerType = typeof(T);
			Instance = new Id<T>(InnerType.Name.Replace("`1", string.Empty));
		}
		public IEnumerator<string> Sequence()
			=> Enumerable.Range(0, int.MaxValue).Select((Func<int, string>)(s
			=> (string)Instance.Append(s).Value)).GetEnumerator();

		public Id<T> Append(object appendValue) => Instance with { Value = $"{Value}.{appendValue}" };
		public Id<T> Combine(params object[] ids) => Instance with { Value = string.Join('.', ids.ToArray()) };

		public static Id<T> Create(object id)
			=> Instance with { Value = $"{id}" };
		public static Id<T> Join(params object[] ids)
			=> Instance with { Value = string.Join('.', ids) };
	}

	public record Id<T, T2>(string unitId, string entityId) : Id<T>(string.Join('.', unitId, entityId))
	{
		public static new readonly Id<T, T2> Instance = new Id<T, T2>(typeof(T).Name, typeof(T2).Name);
	}

	public record Id(string Value) : IEquatable<Id>
	{
		public virtual bool Equals(Id other)
			=> Value == other?.Value == true;

		public override int GetHashCode()
			=> Value.GetHashCode();

		public static Id operator +([NotNull] Id _this, Id other)
			=> _this with { Value = string.Join('.', _this.Value, other.Value) };

		public static implicit operator string(Id id)
			=> id.Value;

		public static implicit operator long(Id id)
			=> long.Parse(id.Value, CultureInfo.CurrentCulture);

		public static implicit operator int(Id id)
			=> int.Parse(id.Value, CultureInfo.CurrentCulture);

		public static implicit operator Id(int id)
			=> new Id($"{id}");

		public static implicit operator Id(long id)
			=> new Id($"{id}");
	}
}

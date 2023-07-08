using System.Collections.Specialized;

namespace MissBot.Abstractions.Bot
{
	public abstract class BaseMetaData : ListDictionary, IMetaData
	{
		public BaseMetaData() : base(StringComparer.OrdinalIgnoreCase)
		{ }

		public BaseMetaData(IUnit rootUnit) : base(StringComparer.OrdinalIgnoreCase)
		{ }

		public IEnumerable<string> Paths { get; }
		public IEnumerable<string> Names { get; }
		public abstract object this[string key] { get; }
	}
}

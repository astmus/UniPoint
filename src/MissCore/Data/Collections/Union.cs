using System.Text.Json.Nodes;
using MissBot.Abstractions.Bot;
using System.Linq;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{
	[JsonArray]
	public class Union<TUnit> : DataUnit<TUnit>.UnitCollection where TUnit : class
	{
		public Union(IEnumerable<IUnit<TUnit>> items) : base(items)
		{
		}

		public Union(IEnumerable<JToken> items) : base(items)
		{
		}

		public Union(JArray items) : base(items)
		{
		}
	}
}

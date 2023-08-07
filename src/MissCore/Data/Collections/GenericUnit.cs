using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissBot.Abstractions;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{
	public interface IGenericUnit
	{
		IEnumerable<string> ItemNames { get; }
		IEnumerable<string> ItemValues { get; }
		string StringValue { get; }
		IEnumerator<IUnitItem> GetEnumerator();
	}

	[JsonDictionary]
	public class GenericUnit : ListDictionary, IGenericUnit, IBotEntity
	{
		public IEnumerable<string> ItemNames
			=> Keys.Cast<string>();
		public IEnumerable<string> ItemValues
			=> Values.Cast<string>();

		public string StringValue
			=> string.Join(" ", ItemNames.Select(key => $"{key}: {this[key]}"));

		public string Entity { get; }
		public string UnitKey { get; }

		public new IEnumerator<IUnitItem> GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}

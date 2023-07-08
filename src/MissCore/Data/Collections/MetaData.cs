using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using LinqToDB;

using MissBot.Abstractions;
using MissBot.Abstractions.Bot;

using Newtonsoft.Json.Linq;
namespace MissCore.Data
{
	[JsonDictionary]
	internal class MetaData<TUnit> : Unit.MetaData, IMetaData where TUnit : class
	{
		public MetaData()
		{
		}

		public MetaData(object unit)
		{
			ParseTokens(JToken.FromObject(unit));
		}



		public MetaData<TUnit> ParseRoot(JToken containerToken)
		{
			if (containerToken == null)
				return null;
			return ParseTokens(containerToken);
		}


		public override object this[string key]
			=> root[key] switch
			{
				JProperty prop => prop.Value,
				JValue value => value.Value,
				JObject obj => obj.GetValue(key),
				_ => throw new ArgumentException("key is not path to property or value")
			};


		protected override MetaData<TUnit> ParseTokens(JToken containerToken)
		{
			switch (containerToken)
			{
				case JObject obj:
					ParseObject(obj);
					break;
				case JArray arr:
					ParseArray(arr);
					break;
				case JProperty pro:
					ParseProperty(pro);
					break;
				default:
					ParseValue(containerToken as JValue);
					break;
			}
			root = containerToken;
			return this;
		}
	}

	public partial record Unit : BaseUnit
	{
		public static IMetaData ReadMetadata<T>(T data) where T : class
			=> data is not null ? new MetaData().Parse(data) : null;

		public static TUnit Init<TUnit, TData>(TData data) where TUnit : Unit<TData> where TData : class
		{
			var unit = Activator.CreateInstance<TUnit>();
			unit.SetContext(data);
			return unit;
		}

		public override void SetContext(object data)
		{
			throw new NotImplementedException();
		}

		[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
		[JsonDictionary]
		internal partial class MetaData : ListDictionary, IMetaData
		{
			protected JToken root { get; set; }
			public MetaData() : base(StringComparer.OrdinalIgnoreCase)
			{
			}

			public MetaData(IUnit rootUnit) : base(StringComparer.OrdinalIgnoreCase)
			{
			}

			public IEnumerable<string> Paths
				=> Values.Cast<string>();

			public IEnumerable<string> Names
				=> Keys.Cast<string>();

			public virtual object this[string key]
			{
				get => base[key];
			}

			public MetaData Parse<T>(T data) where T : class
			{
				if (data != null)
					if (data is JToken token)
						return ParseTokens(token);
					else
						return ParseTokens(JToken.FromObject(data));
				return this;
			}

			protected MetaData ParseValue(JValue token)
			{
				Add("Content", token);
				return this;
			}
			protected MetaData ParseProperty(JProperty token)
			{
				Add(token.Name, token.Path);
				//root = token;
				return this;
			}

			protected MetaData ParseArray(JArray token)
			{
				foreach (var children in token.Children())
				{
					foreach (var child in children.Children<JProperty>())
					{
						Add(child.Name, child.Path);
					}
					break;
				}
				return this;
			}

			protected virtual MetaData ParseObject(JObject token)
			{
				foreach (var child in token.Children<JProperty>())
				{
#if DEBUG
					Console.WriteLine($"{child.Name}:{child.Path}");
#endif
					Add(child.Name, child.Path);
				}
				return this;
			}

			protected virtual MetaData ParseTokens(JToken token) => token switch
			{
				JObject obj => ParseObject(obj),
				JArray arr => ParseArray(arr),
				JProperty prop => ParseProperty(prop),
				JValue val => ParseValue(val),
				_ => this
			};

			protected void InvalidateRoot()
				=> ParseTokens(root);

			public void SetRoot<TRoot>(TRoot rootObject) where TRoot : JToken
			{
				root = rootObject;
				InvalidateRoot();
			}

			private string GetDebuggerDisplay()
				=> string.Join(" ", Names.Select(key => $"{key}: {this[key]}"));
		}
	}
}

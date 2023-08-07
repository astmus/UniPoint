using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissBot.Extensions;
using MissBot.Identity;
using MissCore.Data.Collections;
using MissCore.Data.Entities;
using MissCore.Internal;
using Newtonsoft.Json.Linq;

namespace MissCore.Data
{
	//[Table("##BotUnits")]
	//public partial record Unit : BaseUnit, IUnit//, IBotIdentible
	//{
	//	//[Column("Unit")]
	//	//public override string Unit { get; set; }

	//	public override IEnumerator UnitEntities { get; }
	//	[Column("Id")]
	//	public override object Identifier { get; set; } = Id<Unit>.Instance.Key;
	//	//public static TMeta FromObject<TMeta>(object dataObject) where TMeta : class, IUnitContext
	//	//{
	//	//    var meta = Activator.CreateInstance<TMeta>();
	//	//    meta.
	//	//    meta.SetRoot(JToken.FromObject(dataObject));
	//	//    return meta;
	//	//}

	//	internal partial class MetaData : ListDictionary
	//	{
	//		public static MetaData Create([System.Diagnostics.CodeAnalysis.NotNull] object dataObject)
	//		{
	//			var meta = new MetaData();
	//			meta.SetRoot(JToken.FromObject(dataObject));
	//			return meta;
	//		}

	//		public static TMeta FromObject<TMeta>([System.Diagnostics.CodeAnalysis.NotNull] object dataObject) where TMeta : MetaData
	//		{
	//			var meta = Activator.CreateInstance<TMeta>();
	//			meta.SetRoot(JToken.FromObject(dataObject));
	//			return meta;
	//		}

	//		//public static TMeta FromRootToken<TMeta>(JToken dataObject) where TMeta : MetaData
	//		//{
	//		//	var meta = Activator.CreateInstance<TMeta>();
	//		//	meta.SetRoot(dataObject);
	//		//	return meta;
	//		//}
	//	}
	//}


	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	[JsonConverter(typeof(ValueUnitConverter))]
	public readonly record struct ValueUnit(object Value) : IValueUnit
	{
	}

	/// <summary>
	/// [DebuggerDisplay($"Value: {nameof(Text)}")]
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <param name="Entity"></param>

	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	//[JsonConverter(typeof(GenericUnitConverter<Unit>))]
	public record DataUnit<TData> : Unit, IUnit<TData>, IDataUnit<TData> where TData : class
	{
		public DataUnit()
		{

		}
		public static readonly string Key = Id<TData>.Instance.Value;
		Lazy<TData> _lazyEntity;

		public override string Unit { get; set; } = Id<TData>.Instance.Value;

		//[Column]
		//public override string Unit { get; set; } = Key;

		[JsonProperty(Order = int.MinValue + 1)]
		public virtual string Entity { get; set; }

		[JsonProperty("Item", Order = int.MinValue + 2)]
		public virtual TData UnitData
			=> _lazyEntity?.Value;


		public override object Identifier
			=> Id<TData>.Join(Unit);

		[JsonIgnore]
		public IUnitContext DataContext { get; set; }

		[JsonIgnore]
		public override IEnumerator UnitEntities
			=> DataContext.UnitEntities;

		public IEnumerable<IEnumerable<IUnitAction<TData>>> Actions { get; set; }

		public virtual void SetDataContext(TData data)
		{
			DataContext = new DataUnit<TData>.UnitContext(data);
			_lazyEntity = new Lazy<TData>(() =>
			{
				return DataContext.GetUnitEntity<TData>();
			});
		}

		public static DataUnit<TData> Init<TUData>(TUData data)
		{
			var unit = Activator.CreateInstance<DataUnit<TData>>();
			unit.SetDataContext(JToken.FromObject(data));
			return unit;
		}

		public static DataUnit<TUData>.UnitCollection InitCollection<TUData>(TUData data) where TUData : class
		{
			var collection = Activator.CreateInstance<DataUnit<TUData>.UnitCollection>();
			collection.Add(DataUnit<TUData>.Init(data));
			//unit.Data = FromRootToken<TData>(JToken.FromObject(data));
			return collection;
		}

		public static IMetaData ReadMetadata(TData data)
			=> new DataUnit<TData>.UnitContext(data);

		public static IMetaData ReadMetadata<TUnit>(object data) where TUnit : class
			=> data is JToken token ? MetaData.FromRootToken<MetaData<TUnit>>(token) : new MetaData<TUnit>(data);


		public void SetDataContext<TUnit>(TUnit unit) where TUnit : JToken
		{
			DataContext = new DataUnit<TData>.UnitContext(unit);
		}

		internal class UnitContext : MetaData<TData>, /*IDataUnit<TData>,*/ IUnitContext
		{
			public UnitContext()
			{
			}
			public UnitContext(JToken token)
			{
				SetRoot(token);
			}
			public UnitContext(TData data) : base(data)
			{
			}

			public IUnitContext DataContext { get; set; }
			public IEnumerator UnitEntities
				   => root.Children().GetEnumerator();

			public JToken Root
				=> root;

			public TData UnitData
				=> GetUnitEntity<TData>();

			public void SetContext<TUnitData>(TUnitData data) where TUnitData : JToken
			{
				SetRoot(data);
			}

			public TEntity GetUnitEntity<TEntity>() where TEntity : class
				=> root.ToObject<TEntity>();
		}

		[JsonArray]
		public class UnitCollection : MetaCollection<TData>, IUnitCollection<TData>, IEnumerable<TData>
		{
			public readonly static new UnitCollection Empty = new UnitCollection(Enumerable.Empty<JToken>());
			public UnitCollection() : base()
			{
			}
			public UnitCollection(IEnumerable<IUnit<TData>> items) : base(items)
			{
			}

			public UnitCollection(IEnumerable<JToken> items) : base(items)
			{
			}

			public UnitCollection(JArray items) : base(items)
			{
			}

			[JsonProperty("replay_markup")]
			public IUnitActions<TData> Actions { get; set; }

			public void Add<TUnit>(TUnit unit) where TUnit : IUnit<TData>
				=> base.Add(unit);

			public void Add(IUnit<TData> unit)
			{
				Add(unit);
			}

			public new virtual IEnumerable<TUnit> Enumarate<TUnit>() where TUnit : class
			{
				foreach (var token in this)
				{
					switch (token)
					{
						case JObject obj:
							var result = token.ToObject<TData>();
							if (result is ResultUnit<TData> unit)
							{
								unit.DataContext = Data.Unit.MetaData.FromRootToken<DataUnit<TData>.UnitContext>(obj);
							}
							break;
						case JProperty prop:
							var u = PropertyFacade.Instance with { context = prop };
							yield return token.ToObject<TUnit>();
							break;
					}
				}
				yield return default;

			}

			//public IEnumerable<TAction> EnumarateActions<TAction>(IIdentible context) where TAction : IUnitAction<TData>
			//{

			//}
		}
	}
}

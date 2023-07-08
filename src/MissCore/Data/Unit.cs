using System.Collections;
using System.Collections.Specialized;
using System.Text.Json.Nodes;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using MissCore.Data.Collections;
using MissCore.Data.Entities;
using Newtonsoft.Json.Linq;

namespace MissCore.Data
{
	//[Table("##BotUnits")]
	public partial record Unit : BaseUnit, IUnit//, IBotIdentible
	{
		[Column("Unit")]
		public override string UnitKey { get; set; }

		public override IEnumerator UnitEntities { get; }

		public override object Identifier
			=> Id<Unit>.Instance.Key;
		//public static TMeta FromObject<TMeta>(object dataObject) where TMeta : class, IUnitContext
		//{
		//    var meta = Activator.CreateInstance<TMeta>();
		//    meta.
		//    meta.SetRoot(JToken.FromObject(dataObject));
		//    return meta;
		//}

		internal partial class MetaData : ListDictionary
		{
			public static MetaData Create([System.Diagnostics.CodeAnalysis.NotNull] object dataObject)
			{
				var meta = new MetaData();
				meta.SetRoot(JToken.FromObject(dataObject));
				return meta;
			}

			public static TMeta FromObject<TMeta>([System.Diagnostics.CodeAnalysis.NotNull] object dataObject) where TMeta : MetaData
			{
				var meta = Activator.CreateInstance<TMeta>();
				meta.SetRoot(JToken.FromObject(dataObject));
				return meta;
			}

			public static TMeta FromRootToken<TMeta>(JToken dataObject) where TMeta : MetaData
			{
				var meta = Activator.CreateInstance<TMeta>();
				meta.SetRoot(dataObject);
				return meta;
			}
		}
	}
	/// <summary>
	/// [DebuggerDisplay($"Value: {nameof(Text)}")]
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <param name="Entity"></param>

	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
	//[JsonConverter(typeof(GenericUnitConverter<Unit>))]
	public record Unit<TData> : Unit, IUnit<TData>, IUnitContext<TData>, IInteractableUnit<TData> where TData : class
	{
		public static readonly string Key = Id<TData>.Instance.Key;
		Lazy<TData> _lazyEntity;

		[Column("Unit")]
		public override string UnitKey { get; set; } = Key;

		[JsonProperty("Entity", Order = int.MinValue + 1)]
		public virtual string EntityKey { get; set; }

		[JsonProperty("Item", Order = int.MinValue + 2)]
		public virtual TData UnitData
			=> _lazyEntity?.Value;

		[JsonIgnore]
		public override object Identifier
			=> Id<TData>.Join(UnitKey, EntityKey, DataContext["Id"]);

		[JsonIgnore]
		public IUnitContext DataContext { get; set; }

		[JsonIgnore]
		public override IEnumerator UnitEntities
			=> DataContext.UnitEntities;

		public IEnumerable<IEnumerable<IUnitAction<TData>>> Actions { get; set; }

		public override void SetContext(object data)
		{
			DataContext = Unit.MetaData.FromObject<UnitContext>(data);
		}
		//public override void SetContext<TUnitData>(TUnitData data)
		//{
		//    base.SetRawData<TUnitData>(data);
		//    _lazyEntity = new Lazy<TData>(() =>
		//    {
		//        UnitData.ThrowIfNull(nameof(UnitData));
		//        UnitData.ThrowIfTypeIs(nameof(UnitData), typeof(JValue), typeof(JArray));
		//        return UnitData is JObject || UnitData is JProperty || UnitData is not JToken ? UnitData?.ToObject<TData>() : null;
		//    });
		//    MetaData = ReadMetadata<TData>(data);
		//}

		public static Unit<TData> Init<TUData>(TUData data) where TUData : class
		{
			var unit = Activator.CreateInstance<Unit<TData>>();
			unit.SetContextRoot(JToken.FromObject(data));
			return unit;
		}

		public static Unit<TUData>.UnitCollection InitCollection<TUData>(TUData data) where TUData : class
		{
			var collection = Activator.CreateInstance<Unit<TUData>.UnitCollection>();
			collection.Add(Unit<TUData>.Init(data));
			//unit.Data = FromRootToken<TData>(JToken.FromObject(data));
			return collection;
		}

		public static IMetaData ReadMetadata(TData data)
			=> Unit.MetaData.FromObject<UnitContext>(data);

		public static IMetaData ReadMetadata<TUnit>(object data) where TUnit : class
			=> data is JToken token ? MetaData.FromRootToken<MetaData<TUnit>>(token) : MetaData.FromObject<MetaData<TUnit>>(data);

		public void SetContextRoot<TRoot>(TRoot data) where TRoot : JToken
		{
			DataContext = Unit.MetaData.FromObject<UnitContext>(data);
		}

		internal class UnitContext : MetaData<TData>, IUnitContext<TData>, IUnitContext
		{
			public IUnitContext DataContext { get; set; }
			public IEnumerator UnitEntities
				   => root.Children().GetEnumerator();

			public JToken Root
				=> root;

			public UnitContext()
			{
			}
			public UnitContext(TData data) : base(data)
			{
			}

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

			public override IEnumerable<TUnit> Enumarate<TUnit>()
			{
				foreach (var token in this)
				{
					var result = token.ToObject<TUnit>();
					if (result is ResultUnit<TData> unit)
					{
						unit.DataContext = Unit.MetaData.FromRootToken<UnitContext>(token);
						//if (result is IInteractableUnit interUnit)
						//{
						//    //unit.Entities = null;
						//    interUnit.ActionsSet = this.Actions.GetSet(unit.Id);
						//}
					}
					yield return result;
				}
			}

			//public IEnumerable<TAction> EnumarateActions<TAction>(IIdentible context) where TAction : IUnitAction<TData>
			//{

			//}
		}
	}
}

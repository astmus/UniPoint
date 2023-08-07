using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissCore.Data.Entities;
using MissCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MissCore.Data.Collections
{
	[JsonArray]
	public class MetaCollection<TData> : MetaCollection, IMetaCollection<TData> where TData : class
	{
		public readonly static new MetaCollection<TData> Empty = new MetaCollection<TData>();

		#region Constructors
		public MetaCollection(params object[] content) : base(content) { }
		public MetaCollection(IEnumerable<JToken> items) : base(items)
		{
		}

		public MetaCollection() : base()
		{
		}

		public MetaCollection(JArray items) : base(items)
		{
		}

		public MetaCollection(object content) : base(content)
		{
		}
		#endregion

		protected override void Init()
		{
			if (ChildrenTokens.FirstOrDefault() is JToken token)
				MetaData = DataUnit<TData>.ReadMetadata<TData>(token);
		}
		public virtual IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity : class, TData
		{
			foreach (var token in this)
			{
				var result = token.ToObject<TEntity>();
				yield return result;
			}
		}

		public IEnumerable<TUnit> EnumarateUnits<TUnit>() where TUnit : BaseUnit, IUnit<TData>
		{
			foreach (var token in this)
			{
				var result = token.ToObject<TUnit>();
				result.SetDataContext(token);
				yield return result;
			}
		}
		public override IEnumerable<IUnitItem> EnumarateUnitItems()
		{

			foreach (JObject obj in this)
			{
				foreach (JProperty prop in obj.Properties())
				{
					var u = PropertyFacade.Instance with { context = prop };
					var result = obj.ToObject<TData>();
					if (result is ResultUnit<TData> unit)
					{
						unit.DataContext = Data.Unit.MetaData.FromRootToken<DataUnit<TData>.UnitContext>(obj);
					}
					yield return u;
				}
			}

		}
		IEnumerator<TData> IEnumerable<TData>.GetEnumerator()
		{
			return SupplyTo<TData>().GetEnumerator();
		}



	}
	[JsonDictionary]
	public class MetaCollection : JArray, IMetaCollection, IList<JToken>
	{
		public readonly static MetaCollection Empty = new MetaCollection();

		public IMetaData MetaData { get; protected set; }
		public string Unit
			=> this.FirstOrDefault()?.Value<string>() ?? nameof(MetaCollection);

		public MetaCollection(IEnumerable<JToken> items) : base(items.ToArray())
			=> Init();

		public MetaCollection(JArray items) : base(items)
			=> Init();

		public MetaCollection()
			=> Init();

		public MetaCollection(params object[] content) : base(content)
			=> Init();

		public MetaCollection(object content) : base(content)
			=> Init();

		protected virtual void Init()
			=> MetaData = Data.Unit.ReadMetadata<JToken>(this.FirstOrDefault<JToken>());

		protected virtual IEnumerable<TSub> SupplyTo<TSub>() where TSub : class
		{
			foreach (var token in this)
			{
				yield return token.ToObject<TSub>();
			}
		}



		public virtual IEnumerable<IUnitItem> EnumarateUnitItems()
		{
			throw new NotImplementedException();
		}




		public IEnumerable<TUnit> Enumarate<TUnit>() where TUnit : class
		{
			foreach (var token in this)
			{
				var result = token.ToObject<TUnit>();

				if (result is IUnit<TUnit> unit)
					unit.SetDataContext(token);

				yield return result;
			}
			//}
		}
	}
}



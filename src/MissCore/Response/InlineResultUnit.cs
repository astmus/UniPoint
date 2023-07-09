using LinqToDB.Mapping;

using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Results.Inline;
using MissBot.Identity;

using MissCore.Data;
using MissCore.Data.Entities;

using Newtonsoft.Json.Linq;

namespace MissCore.Response
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
	[Table("##SearchResults")]
	public record InlineResultUnit<TEntity> : ResultUnit<TEntity>, IUnit<TEntity>, IBotUnit<TEntity>, IInteractableUnit<TEntity> where TEntity : class, IUnit
	{
		[JsonProperty]
		public InlineQueryResultType Type { get; set; } = InlineQueryResultType.Article;

		[JsonProperty("input_message_content")]
		public override IInlineContent Content
		{
			get
			{
				var r = DataContext.GetUnitEntity<InlineContent<TEntity>>();
				r.Value = string.Join(' ', DataContext.Root.Children().Values<string>());
				return r;
			}
		}

		[Column]
		[JsonProperty]
		public virtual string Id { get; set; } = Id<TEntity>.Instance;
		//=> DataContext[nameof(Id)];

		public string QueryId { get; set; }

		[Column]
		[JsonProperty]
		public override string Title { get; set; }

		[Column]
		[JsonProperty]
		public override string Description { get; set; }

		//[JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
		//public IUnitActionsSet UnitActions { get; set; }

		[Column("Entity", IsDiscriminator = true)]
		[JsonProperty("Entity")]
		public override string Entity { get; set; }

		[JsonProperty("Unit")]
		[Column("Unit")]
		public override string Unit { get; set; }

		public override object Identifier
			=> Id;

		public IEnumerable<IEnumerable<IUnitAction<TEntity>>> Actions { get; set; }


		public override void SetContext<TDataUnit>(TDataUnit data)
		{
			DataContext = new Unit<TDataUnit>.UnitContext(data);
		}

		public override void SetContext(object data)
		{
			throw new NotImplementedException();
		}

		public override void SetContextRoot<TRoot>(TRoot data)
		{
			DataContext = new Unit<TRoot>.UnitContext(data);
		}

		//public override void SetContext<TUnitData>(TUnitData data)
		//{
		//    base.SetRawData<TUnitData>(data);
		//    //UnitData = data;
		//    MetaData = Unit<TEntity>.ReadMetadata<TEntity>(data);
		//}

	}
}

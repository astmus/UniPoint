using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Query;

using MissCore.Bot;
using MissCore.Data.Entities;
using MissBot.Identity;

namespace MissCore.Response
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record InlineResponse<TUnit>(IHandleContext Context = default) : AnswerInlineRequest<TUnit>, IResponse<TUnit> where TUnit : BaseUnit
	{
		InlineQuery InlineQuery
			=> Context.Take<InlineQuery>();

		[JsonProperty(Required = Required.Always)]
		public override string InlineQueryId
			=> InlineQuery.Id;

		[JsonProperty(Required = Required.Always)]
		public override ICollection<ResultUnit<TUnit>> Results { get; init; } = new List<ResultUnit<TUnit>>();

		public override string NextOffset
			=> Results?.Count < Pager.PageSize ? null : Convert.ToString(Pager.Page + 1);

		public override int? CacheTime { get; set; } = 300;
		public int Length
			=> Results.Count;
		public Paging Pager { get; set; }

		public async Task Commit(CancellationToken cancel)
		{
			if (Results.Count > 0)
				try
				{
					await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
				}
				finally
				{
					Results.Clear();
				}
		}

		public void WriteUnit<TData>(TData unit) where TData : class, IUnit<TUnit>
		{
			if (unit is InlineResultUnit<TUnit> item)
			{
				var entities = unit.UnitEntities;
				entities.MoveNext();
				item.Id = Id<TUnit>.Instance.Combine(unit.Unit, unit.Identifier, InlineQuery.Query);
				item.QueryId = InlineQuery.Query;
				item.Title ??= entities.Current.ToString();
				entities.MoveNext();
				item.Description ??= entities.Current.ToString();
				// item.Content.Value = item.Data.StringValue;
				Results.Add(item);
			}
		}
		public void AddUnit<TData>(IUnit<TData> unit) where TData : class, TUnit
		{
			var item = Context.BotServices.Activate<InlineResultUnit<TUnit>>();
			item.SetContext(unit);
			var entities = unit.UnitEntities;
			entities.MoveNext();
			item.Id = Id<TUnit>.Instance.Combine(unit.Identifier).Combine(InlineQuery.Query);
			item.Title ??= entities.Current.ToString();
			entities.MoveNext();
			item.Description ??= entities.Current.ToString();
			//item.Content.Value = item.Data.StringValue;
			Results.Add(item);
		}

		public void AddUnits<TData>(IEnumerable<TUnit> units) where TData : class, IUnit<TUnit>
		{
			foreach (IUnit<TUnit> unit in units)
				if (unit is ResultUnit<TUnit> result)
					WriteUnit(unit);
		}

		void IResponse<TUnit>.WriteMetadata<TData>(TData meta)
		{
			throw new NotImplementedException();
		}

		public IResponse Write(object data)
		{
			throw new NotImplementedException();
		}

		void IResponse<TUnit>.AddUnits<TData>(IEnumerable<TData> units)
		{
			throw new NotImplementedException();
		}
	}
}



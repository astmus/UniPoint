using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissBot.Response;
using MissCore.Bot;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineResponse<T>(IHandleContext Context = default) : AnswerInlineRequest<T>, IResponse<T> where T : InlineQuery
    {
        T InlineQuery
            => Context.Take<T>();

        public override string InlineQueryId
            => InlineQuery.Id;
        public override IEnumerable<InlineResultUnit> Results
            => results;

        List<InlineResultUnit> results = new List<InlineResultUnit>();
        
        public override string NextOffset
            => results?.Count < Pager.PageSize ? null : Convert.ToString(Pager.Page+1);
        public override int? CacheTime { get; set; } = 300;
        public int Length
            => results.Count;
        public string Content { get; set; }
        public Paging Pager { get; set; }

        public async Task Commit(CancellationToken cancel)
        {
            if (results.Count > 0)
            await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false) ;
        }

        public void Write<TUnitData>(TUnitData unit) where TUnitData : UnitBase
        {
            if (unit is InlineResultUnit item)
            {
                item.Id += InlineQuery.Query;
                item.Title ??= item.Meta.GetItem(2).ToString();
                item.Description ??= item.Meta.GetItem(3).ToString();
                results.Add(item);
            }
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : UnitBase
        {
            foreach (var unit in units)
            {
                if (unit is InlineResultUnit result)
                {
                    Write(unit);
                }
            }
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : class, IMetaData
        {
            throw new NotImplementedException();
        }

        public IResponse<T> InputData(string description, IActionsSet options = null)
        {
            return this;
        }

        public IResponse CompleteInput(string message)
        {
            return this;
        }
    }


}



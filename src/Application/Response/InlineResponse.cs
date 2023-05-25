using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissBot.Response;

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
            => results?.Count < InlineQuery.BatchSize ? null : Convert.ToString(InlineQuery.BatchSize);
        public override int? CacheTime { get; set; } = 300;
        public int Length
            => results.Count;

        public string Content { get; set; }

        public async Task Commit(CancellationToken cancel)
        {
            await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false) ;
        }

        public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit, IUnit<T>
        {
            if (unit is InlineResultUnit item)
            {
                item.Id += InlineQuery.Query;
                item.Title ??= item.Meta.GetItem(2).ToString();
                item.Description ??= item.Meta.GetItem(3).ToString();
                results.Add(item);
            }
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit, IUnit<T>
        {
            foreach (var unit in units)
            {
                if (unit is InlineResultUnit result)
                {
                    Write(unit);
                }
            }
        }

        public void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<IUnit<T>>
        {
            //foreach (var item in unit)
            //    Write(item);
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : class, IMetaData
        {
            throw new NotImplementedException();
        }

        public void WriteError<TUnitData>(TUnitData unit) where TUnitData : class, IUnit
        {
            //results.Add(InitResult(unit));
        }

        public void WriteSimple<TUnitData>(TUnitData unit) where TUnitData :class, IUnit
        {

        }

        public IResponse<T> InputRequest(string description, IActionsSet options = null)
        {
            return this;
        }


    }


}



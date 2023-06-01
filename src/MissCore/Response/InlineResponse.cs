using BotService.Internal;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore.Bot;
using Newtonsoft.Json.Converters;

namespace MissCore.Response
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineResponse<TUnit>(IHandleContext Context = default) : AnswerInlineRequest<TUnit>, IResponse<TUnit> where TUnit : InlineQuery, IBotEntity// BaseUnit, IBotEntity
    {
        InlineQuery InlineQuery
            => Context.Take<InlineQuery>();

        public override string InlineQueryId
            => InlineQuery.Id;
        public override IEnumerable<ResultUnit> Results
            => results;

        List<ResultUnit> results = new List<ResultUnit>();

        public override string NextOffset
            => results?.Count < Pager.PageSize ? null : Convert.ToString(Pager.Page + 1);
        public override int? CacheTime { get; set; } = 300;
        public int Length
            => results.Count;

        public Paging Pager { get; set; }


       public  TUnit Content { get; set; }
        IUnit<TUnit> IResponse<TUnit>.Content { get; set; }

        public async Task Commit(CancellationToken cancel)
        {
            if (results.Count > 0)
                await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
        }

        
        public void Write<TData>(TData unit) where TData :IUnit<TUnit>
        {
            if (unit is ResultUnit item)
            {
                item.Id += InlineQuery.Query;
                item.Title ??= item.Meta.GetItem(2).Serialize();
                item.Description ??= item.Meta.GetItem(3).Serialize();
                results.Add(item);
            }
        }




        public IResponse CompleteInput(string message)
        {
            throw new NotImplementedException();
        }
        public void Write<TData>(IEnumerable<TData> units) where TData :IUnit< TUnit>
        {
        foreach(var unit in units)
            {
                if (unit is ResultUnit result)
                {
                    //Write(unit);
                }
            }
        }

        void IResponse<TUnit>.WriteMetadata<TData>(TData meta)
        {
            throw new NotImplementedException();
        }

        IResponse<TUnit> IResponse<TUnit>.InputData(string description, IActionsSet options)
        {        
            return this;
        }        
        //public IResponse<T> InputData(string description, IActionsSet options = null)
        //{
        //}


    }


}



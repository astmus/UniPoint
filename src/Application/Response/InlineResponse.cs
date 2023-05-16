using MissBot.Abstractions;
using MissBot.Entities.Query;
using MissBot.Entities.Results.Inline;
using MissBot.Response;
using MissCore.Collections;

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
        InlineKeyBoard keyboard;
        InlineKeyBoard Keyboard
        => keyboard ?? (keyboard = new InlineKeyBoard());


        public override string NextOffset
            => results?.Count < 50 ? null : "50";
        public override int? CacheTime { get; set; } = 5;

        public async Task Commit(CancellationToken cancel)
        {
            //Button = new InlineQueryResultsButton("SearchOption") { StartParameter = "Parameter" };
            await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);
        }

        public void Write<TUnitData>(TUnitData unit) where TUnitData : class, IUnit<T>
        {
            var item = unit as InlineResultUnit;
            item.Id += InlineQuery.Query;
            item.Title = item.Meta.GetItem(2).ToString();
            item.Description = item.Meta.GetItem(3).ToString();
            results.Add(item);
            keyboard = null;
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : class, IUnit<T>
        {
            foreach (var unit in units)
                Write(unit);
        }

        InlineQueryResult InitResult(object data) => data switch
        {
            string weak => new InlineQueryResultArticle(null, null, new InputTextMessageContent(weak)),
            IInlineUnit unit => new InlineQueryResultArticle(null, null, new InputTextMessageContent(Convert.ToString(unit))),
            Unit error => new InlineQueryResultArticle(Convert.ToString(error) ?? "-1", "Error", new InputTextMessageContent(error.ToString())),
            _ => null
        };

        public void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<IUnit>
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
    }


}



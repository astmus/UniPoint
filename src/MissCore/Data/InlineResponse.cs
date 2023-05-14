using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Common;
using MissBot.Entities.Query;
using MissBot.Entities.Results.Inline;


namespace MissBot.Entities.Response
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineResponse<T>(IHandleContext Context = default) : InlineResult<T>, IResponse<T> where T : InlineQuery
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


        //public override string NextOffset
        //    => Results?.Count() < 50 ? null : "50";
        public override int? CacheTime { get; set; } = 5;

        public async Task Commit(CancellationToken cancel)
            => await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);
                                   

        public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit<T>
        {

            //var meta = Unit<TUnitData>.MapData(unit);
            //IInlineUnit inline = unit as IInlineUnit;
            //var content = inline.Content;// meta.AnyFirst("Content");
            // InlineQueryResult result = InitResult(content);
            var item = unit as InlineResultUnit;
            item.Title = item.Meta.GetRecord(1, "{0}: {1}");
            item.Description = item.Meta.GetRecord(2, "{0}: {1}");
            results.Add(item);
            //foreach (var item in unit)
            //{
            //    //var key = item.ToString();

            //    SetContent(item.ToString(), unit, result, meta);
            //}

            // result.ReplyMarkup = Keyboard?.GetKeyboard;
            //results.Add(result);
            keyboard = null;
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<T>
        {
            foreach (var unit in units)           
                Write(unit);             
        }


        object SetContent(string key, object value, InlineQueryResult result, MetaData meta) => result switch
        {
            InlineQueryResultArticle article when key == nameof(article.Id) => result.Id = article.Id = value + InlineQuery.Query,
            InlineQueryResultArticle article when key == nameof(article.Title) => article.Title = (string)value,
            InlineQueryResultArticle article when key == nameof(article.Description) => article.Description = (string)value,
            InlineQueryResultArticle article when value is BotAction action => Keyboard.Append(action),
            InlineQueryResultArticle article when key == nameof(article.Title) => article.Title = meta["Name"].ToString(),
            //InlineQueryResultArticle article when value is InlineKeyBoard mark => article.ReplyMarkup = mark.GetKeyboard,
            _ => result
        };


        InlineQueryResult InitResult(object data) => data switch
        {
            string weak => new InlineQueryResultArticle(null, null, new InputTextMessageContent(weak)),
            IInlineUnit unit => new InlineQueryResultArticle(null, null, new InputTextMessageContent(Convert.ToString(unit))),
            Unit error => new InlineQueryResultArticle(Convert.ToString(error) ?? "-1", "Error", new InputTextMessageContent(error.ToString())),
            _ => null
        };

        public void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<Unit>
        {
            //foreach (var item in unit)
            //    Write(item);
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : MetaData
        {
            throw new NotImplementedException();
        }

        public void WriteError<TUnitData>(TUnitData unit) where TUnitData : Unit
        {
            //results.Add(InitResult(unit));
        }

        public void WriteSimple<TUnitData>(TUnitData unit) where TUnitData : Unit
        {
            
        }
    }


}



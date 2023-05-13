using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Results.Inline;
using MissBot.Commands.Results.Inline;
using MissCore.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Common
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineResponse<T> : InlineResult<T>, IResponse<T> where T : InlineQuery
    {
        BotClientDelegate Client;
        Message message;
        Chat channel;
        T Query;

        public override string InlineQueryId
            => Query.Id;
        public override IEnumerable<InlineQueryResult> Results
            => results;
        InlineKeyBoard keyboard;
        InlineKeyBoard Keyboard
        => keyboard ?? (keyboard = new InlineKeyBoard());

        List<InlineQueryResult> results = new List<InlineQueryResult>();

        public override string? NextOffset
            => results?.Count < 15 ? null : "15";
        public override int? CacheTime { get; set; } = 5;

        public async Task Commit(CancellationToken cancel)
            =>  await Client().SendQueryRequestAsync(this);
        
        public void Init(ICommonUpdate update, BotClientDelegate sender, T unit = null)
        {
            channel = update.Chat;
            message = update.Message;
            Query = unit;
            Client = sender;
            results.Clear();
        }

        public async Task<IResponseChannel> InitAsync(T data, ICommonUpdate update, BotClientDelegate sender)
        {
            Init(update, sender);
            return await Client().SendQueryRequestAsync(new GetChannelQuery<T>(channel.Id));
        }

        public void Write<TUnitData>(TUnitData unit) where TUnitData: Unit<T>
        {
            
            var meta = Unit<TUnitData>.Get(unit);
            //IInlineUnit inline = unit as IInlineUnit;
            //var content = inline.Content;// meta.AnyFirst("Content");
           // InlineQueryResult result = InitResult(content);


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
            {
                Write(unit);
            }
        }

    
        object SetContent(string key, object value, InlineQueryResult result , MetaData meta) => result switch
            {
                InlineQueryResultArticle article when key == nameof(article.Id) => result.Id=  article.Id = value +Query.Query,
                InlineQueryResultArticle article when key == nameof(article.Title) => article.Title = (string)value,
                InlineQueryResultArticle article when key == nameof(article.Description) => article.Description = (string)value,
                InlineQueryResultArticle article when value is BotAction action => Keyboard.Append(action),
                InlineQueryResultArticle article when key == nameof(article.Title) => article.Title = meta["Name"].ToString(),
                //InlineQueryResultArticle article when value is InlineKeyBoard mark => article.ReplyMarkup = mark.GetKeyboard,
               _ => result
            };           
        
    
        InlineQueryResult InitResult(object? data) => data switch
        {
             string weak => new InlineQueryResultArticle(null, null, new InputTextMessageContent(weak)),
            IInlineUnit unit => new InlineQueryResultArticle(null, null, new InputTextMessageContent(Convert.ToString(unit))),
            Unit error => new InlineQueryResultArticle(Convert.ToString(error.Id) ?? "-1", "Error", new InputTextMessageContent(error.ToString())),
            _ => null
        };

        public void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<ValueUnit>
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
            results.Add(InitResult(unit));
        }
    }


}



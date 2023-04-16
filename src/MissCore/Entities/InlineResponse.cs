using MissBot.Abstractions;
using MissBot.Abstractions.Results.Inline;
using MissBot.Commands.Results.Inline;
using MissCore.Entities;
using Newtonsoft.Json;
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

        public void Write<TUnitData>(TUnitData unit) where TUnitData : ValueUnit
        {
            var meta = unit.GetMetaData();
                var content = meta.AnyFirst("Content");
            InlineQueryResult result = InitResult(content);
          

            foreach (var item in meta)            
                SetContent(item.Key, item.Value, result);

            result.ReplyMarkup = Keyboard?.GetKeyboard;
            results.Add(result);
            keyboard = null;          
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : ValueUnit
        {
            foreach (var unit in units)
            {
                //   results.Add(GetContent(unit));

                //new InlineQueryResultArticle(u.Id, u.Title, new InputTextMessageContent((string)u.Content[0]))
                //{ ReplyMarkup = u.Content[1] as InlineKeyboardMarkup }); ;
                //InlineKeyboardMarkup n = new InlineKeyboardMarkup();
                

            }
        }

    
        object SetContent(string key, object value, InlineQueryResult result) => result switch
            {
                InlineQueryResultArticle article when key == nameof(article.Id) => article.Id = value +Query.Query,
                InlineQueryResultArticle article when key == nameof(article.Title) => article.Title = (string)value,
                InlineQueryResultArticle article when key == nameof(article.Description) => article.Description = (string)value,
                InlineQueryResultArticle article when value is InlineEntityAction action => Keyboard.Append(action),
                //InlineQueryResultArticle article when value is InlineKeyBoard mark => article.ReplyMarkup = mark.GetKeyboard,
                _ => result
            };           
        
    
    InlineQueryResult InitResult(object? data) => data switch
        {
             string weak => new InlineQueryResultArticle(null, null, new InputTextMessageContent(weak)),
            _ => null
        };

        public void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<ValueUnit>
        {
            foreach (var item in unit)
                Write(item);            
        }
    }


}



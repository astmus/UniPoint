using MissBot.Abstractions;
using MissBot.Abstractions.Results.Inline;
using MissBot.Commands.Results.Inline;
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

        List<InlineQueryResult> results = new List<InlineQueryResult>();

        public override string? NextOffset { get; set; } = "15";
        public override int? CacheTime { get; set; } = 5000;

        public async Task Commit(CancellationToken cancel)
        {
            await Client().SendQueryRequestAsync(this);
        }
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
        public void WriteResult<TUnitData>(TUnitData unit) where TUnitData : BotUnion
        {
        }

        public override void Write<TUnitData>(TUnitData unit)
        {
            if (unit is IInlineUnit u)
                results.Add(GetContent(u, u.Content));
                //new InlineQueryResultArticle(u.Id, u.Title, new InputTextMessageContent((string)u.Content[0]))
                //{ ReplyMarkup = u.Content[1] as InlineKeyboardMarkup });
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<T>
        {
            foreach (var unit in units)
            {
                var u = unit as IInlineUnit;
                results.Add(GetContent(u, u.Content));

                //new InlineQueryResultArticle(u.Id, u.Title, new InputTextMessageContent((string)u.Content[0]))
                //{ ReplyMarkup = u.Content[1] as InlineKeyboardMarkup }); ;
            }
        }

        InlineQueryResult GetContent(params object[] content)
        {
            InlineQueryResult result = null;
       
            result = InitResult(content[0]);
            
            foreach (var item in content)
                SetupContent(item, result);
            return result;

        void SetupContent(object value, InlineQueryResult result)
        {
            object value1 = value switch {
                IInlineUnit unit => result.Id = unit.Id,
                InlineKeyboardMarkup markup => result.ReplyMarkup = markup,
                _ => null
            };
        }
        }

        InlineQueryResult InitResult(object value) => value switch
        {
            IInlineUnit unit when unit.Content[0] is string str => new InlineQueryResultArticle(unit.Id,unit.Title, new InputTextMessageContent(str)),            
            _ => null
        };

    }


}



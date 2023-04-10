using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using MissBot.Common;
using MissBot.Extensions.Response;
using Telegram.Bot.Types.InlineQueryResults;
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
        }

        public async Task<IResponseChannel> InitAsync(T data, ICommonUpdate update, BotClientDelegate sender)
        {
            Init(update, sender);
            return await Client().SendQueryRequestAsync(new GetChannelQuery<T>(channel.Id));
        }
        public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit<T>
        {
            if (unit is IInlineUnit u)
                results.Add(new InlineQueryResultArticle(u.Id, u.Title, new InputTextMessageContent((string)u.Content[0]))
                { ReplyMarkup = u.Content[1] as InlineKeyboardMarkup });
        }

        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<T>
        {
            foreach (var unit in units)
            {
                var u = unit as IInlineUnit;
                results.Add(new InlineQueryResultArticle(u.Id, u.Title, new InputTextMessageContent((string)u.Content[0]))
                    { ReplyMarkup = u.Content[1] as InlineKeyboardMarkup });
            }
        }
    }


}



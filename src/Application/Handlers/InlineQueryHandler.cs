using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Configuration;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace MissBot.Handlers
{
    public class InlineQueryHandler : BaseHandler<InlineQuery>
    {
        private static readonly object[] empty = { "Empty" };

        public record DataUnit : BotEntity<InlineQuery>.Unit
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }
        public record ResultUnit(string Id, string Title, object[] Content) : BotEntity<InlineQuery>.Unit, IInlineUnit
        {
            public static ResultUnit Create(DataUnit data, params object[] content)
                => new ResultUnit(data.Id, data.Title, content);

            public static readonly ResultUnit Empty
                = new ResultUnit("0", "No", empty);
        }

        public override Task ExecuteAsync(CancellationToken cancel = default)
            => Task.CompletedTask;

        public async override Task HandleAsync(IContext<InlineQuery> context)
        {
            var data = context.Data;
            var response = context.Response.Create(data);
            response.Init(context.Any<ICommonUpdate>(), context.Root.BotServices.GetRequiredService<IBotClient>, context.Data);
            
            try
            {
                var items = await LoadAsync(data.Offset.IsNullOrEmpty() ? 0 : int.Parse(data.Offset), data.Query);
                if (items.Count() != 0)
                    response.Write(items);
                else
                    response.Write(ResultUnit.Empty);
                await response.Commit(default);
            }
            finally
            {
                context.Root.Set(AsyncHandler);
            }
        }
        public virtual int? BatchSize { get; }
        public  virtual Task<IEnumerable<ResultUnit>> LoadAsync(int skip, string filter)
        {            
            return Task.FromResult(Enumerable.Empty<ResultUnit>().Append(ResultUnit.Empty));
        }

        //data.ChatId,
        //"*PONG*"//,
        //ParseMode.Markdown,
        //replyToMessageId: msg.MessageId,
        //replyMarkup: new InlineKeyboardMarkup(
        //    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
        //)
    }
}

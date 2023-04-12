using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace MissBot.Handlers
{
    public class InlineQueryHandler : BaseHandler<InlineQuery>
    {
        private const string empty = "Empty";

        public record InlineUnit : Unit<InlineQuery>, IInlineUnit
        {
            protected virtual void Refresh() { }
            public string Id { get => Get<string>(); set { Set(value); Refresh(); } }
            public string Title { get => Get<string>(); set => Set(value); }
            public string Content { get => Get<string>(); set => Set(value); }

            public static readonly InlineUnit Empty
                = new InlineUnit() { Id = "", Title = "Not found", Content = empty };
        }
        //public record ResultUnit(string Id, string Title, string Content) : BotEntity<InlineQuery>.Unit, IInlineUnit
        //{
        //    public static ResultUnit Create(InlineUnit data, string filter, string content)
        //        => new ResultUnit(data.Id+filter, data.Title, content);

        //}

        public override Task ExecuteAsync(CancellationToken cancel = default)
            => Task.CompletedTask;

        public async override Task HandleAsync(IContext<InlineQuery> context)
        {
            var data = context.Data;
            var response = context.CreateResponse(data);

            var items = await LoadAsync(data.Offset.IsNullOrEmpty() ? 0 : int.Parse(data.Offset), data.Query);
            if (items.Count() != 0)
                response.WriteResult(items);
            else
                response.Write(InlineUnit.Empty);
            await response.Commit(default);

        }
        public virtual int? BatchSize { get; }
        public virtual Task<BotUnion> LoadAsync(int skip, string filter)
        {
            return Task.FromResult(new BotUnion<InlineUnit>() { InlineUnit.Empty } as BotUnion);
        }

    }
}

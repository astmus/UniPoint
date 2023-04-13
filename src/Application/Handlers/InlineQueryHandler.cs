using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace MissBot.Handlers
{
    public record InlineUnit<TEntity> : Unit<TEntity>, IInlineUnit
    {
        protected override sealed void InvalidateEntityData(TEntity unit)
            => InvalidateMetadate(this, unit);

        protected virtual TEntity InvalidateMetadate(InlineUnit<TEntity> unit, TEntity entity)
            => Value;
        
        //protected override sealed void InvalidateMetadata<TUnit>(TUnit unit, TEntity entity)
        //{
        //    InvalidateMetaData(this, entity);
        //}
        
        public string Id { get => Get<string>(); set { Set(value); } }
        public string Title { get => Get<string>(); set => Set(value); }
        public string Content { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }


    }
    public record InlineUnit : ValueUnit, IInlineUnit
    {
        private const string empty = "Empty";
        public string Id { get => Get<string>(); set { Set(value); } }
        public string Title { get => Get<string>(); set => Set(value); }
        public string Content { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }

        public static readonly InlineUnit Empty
            = new InlineUnit() { Id = "", Title = "Not found", Content = empty };
    }

    public abstract class InlineQueryHandler : BaseHandler<InlineQuery>
    {       

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
        public abstract Task<IEnumerable<ValueUnit>> LoadAsync(int skip, string filter);
        //{
        //    return Task.FromResult(new BotUnion<InlineUnit>() { InlineUnit.Empty } as BotUnion);
        //}

    }
}

using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace MissBot.Handlers
{
    public record InlineUnit<TEntity>(Action<InlineUnit<TEntity>> init = null) : Unit<TEntity>, IInlineUnit where TEntity : InlineUnit<TEntity>
    {
        protected override sealed void InvalidateMetaData(TEntity unit)
        {
            //base.InvalidateMetaData(unit);
            init?.Invoke(this);
        }

        
        
        //protected override sealed void InvalidateMetadata<TUnit>(TUnit unit, TEntity entity)
        //{
        //    InvalidateMetaData(this, entity);
        //}
        
        public int Id { get => Get<int>(); set { Set(value); } }
        public string Title { get => Get<string>(); set => Set(value); }
        public string Content { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }


    }
    public record InlineUnit : ValueUnit, IInlineUnit
    {
        private const string empty = "Empty";
        public int Id { get => Get<int>(); set { Set(value); } }
        public string Title { get => Get<string>(); set => Set(value); }
        public string Content { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }
        
        public static readonly InlineUnit Empty
            = new InlineUnit() { Id = 0, Title = "Not found", Content = empty };
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

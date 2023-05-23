using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore.Collections;

namespace MissCore.Bot
{
    public record Search : BotUnit
    {
        public Search<TEntity> Find<TEntity>(InlineQuery query) where TEntity : class
            => new Search<TEntity>(query,  $"{string.Format(Template, Unit<TEntity>.Key)} {Payload}");
    }

    public class Search<TEntity> : BotUnitRequest<TEntity>, IUnitRequest<TEntity> where TEntity : class
    {
        internal Search(InlineQuery query, string format, IEnumerable<object> info = null) : base(format, info)
        {
            Query = query;
        }

        public InlineQuery Query { get; set; }
        public uint BatchSize { get; set; } = 50;
        //public override string ToString(RequestOptions format = RequestOptions.JsonAuto)

        //    => ToString(format.SnakeTemplate(), default);

        public override string ToString(IFormatProvider? formatProvider)
            => string.Format(Format, Query.Query, Query.Skip, BatchSize);
    }
}

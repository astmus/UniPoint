using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissBot.Entities.Query;
using MissCore.Collections;

namespace MissCore.Bot
{
    public record Paging : BotUnit
    {
        public uint Skip
            => Page * PageSize;
        public uint Page { get; set; } = 0;
        public uint PageSize { get; set; } = 32;
        public override string ToString()
            => string.Format(Payload, Skip, PageSize);       
    }
    public record Search<TUnit> : Search, ISearchUnitRequest<TUnit>
    {
        public RequestOptions RequestOptions { get; set; }
        public string GetCommand(RequestOptions options = RequestOptions.JsonAuto)
        {
            return $"{string.Format(Payload, Query)} {Pager?.ToString()} {options.TrimSnakes()}";
        }
    }
    //public class Search<TEntity> : BotUnitRequest<TEntity>, IUnitRequest<TEntity> where TEntity : class
    //{
    //    internal Search(InlineQuery query, string format, IEnumerable<object> info = null) : base(format, info)
    //    {
    //        Query = query;
    //    }

    //    public InlineQuery Query { get; set; }
    //    public uint BatchSize { get; set; } = 50;
    //    //public override string ToString(RequestOptions format = RequestOptions.JsonAuto)

    //    //    => ToString(format.SnakeTemplate(), default);

    //    public override string ToString(IFormatProvider? formatProvider)
    //        => string.Format(Format, Query.Query, Query.Skip, BatchSize);
    //}
}

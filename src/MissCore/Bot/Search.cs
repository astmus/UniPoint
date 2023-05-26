namespace MissCore.Bot
{
    public record Search : BotUnit
    {
        public string Query { get; set; }
        public Paging Pager { get; set; }
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

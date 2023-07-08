using MissBot.Abstractions.Actions;
using MissCore.DataAccess;

namespace MissCore.Bot
{
    public record Paging : BotUnit
    {
        public uint Skip
            => Page * PageSize;
        public uint Page { get; set; } = 0;
        public uint PageSize { get; set; } = 32;
        public override string ToString()
            => string.Format(Extension, Skip, PageSize);       
    }

    public record Search<TUnit> : Search, ISearchUnitRequest<TUnit>
    {
        public override string GetCommand()
        {
            return $"{string.Format(Extension, Query)} {Pager} {Options.Format()}";
        }
    }
}

using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissBot.Entities;
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
       public override string GetCommand()
        {
            return $"{string.Format(Payload, Query)} {Pager?.ToString()} {Options.Format()}";
        }
    }
}

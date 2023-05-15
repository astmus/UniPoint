using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Query;
using MissCore.Collections;

namespace MissCore.Features
{
    public record Search:Unit;
    public record Search<TEntity> : BotUnit , IRepositoryCommand 
    {
        public InlineQuery Query { get; set; }
        public uint BatchSize { get; set; } = 50;
        public string Request
            => string.Format(Payload, Command, Query.Query, Query.Skip, BatchSize);
    }
    public readonly record struct Filter(int skip, int take, string predicat);

}

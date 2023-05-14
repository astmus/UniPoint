using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;

namespace MissCore.Features
{
    public record Search : BotRequest
    {

    }
    public record Search<TEntity> : BotUnit , IRepositoryCommand 
    {
        public InlineQuery Query { get; set; }
        public uint BatchSize { get; set; } = 50;
        public string Request
            => string.Format(Payload, Command, Query.Query, Query.Skip, BatchSize);
    }
    public readonly record struct Filter(int skip, int take, string predicat);

}

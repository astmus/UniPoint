using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore.Collections;

namespace MissCore.Features
{
    public record Search : Unit, IBotUnit
    {
        public string Entity { get; }
        public string Command { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
        public string Placeholder { get; set; }
    }

    public record Search<TEntity> : BotUnit , IRepositoryCommand 
    {
        public InlineQuery Query { get; set; }
        public uint BatchSize { get; set; } = 50;

        public IRepositoryCommand SingleResult()
            => this;

        public string ToRequest(RequestFormat format = RequestFormat.JsonAuto)
            => format.ApplyTo(this);

        public string ToString(string? format, IFormatProvider? formatProvider)
            => string.Format(Payload, Command, Query.Query, Query.Skip, BatchSize);
        
    }
    public readonly record struct Filter(int skip, int take, string predicat);

}

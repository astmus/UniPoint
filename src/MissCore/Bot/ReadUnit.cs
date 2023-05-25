using MediatR;
using System.Linq.Expressions;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore.Collections;

namespace MissCore.Bot
{
    public record ReadUnit : BotUnit
    {
        public ReadUnit<TEntity> Read<TEntity>() where TEntity : class
            => new ReadUnit<TEntity>() { Payload = string.Format(Payload, Unit<TEntity>.Key) };
    }

    public record ReadUnit<TEntity> : BotUnit, IUnitRequest<TEntity> where TEntity : class
    {
        public RequestOptions RequestOptions { get; set; }
        public override string ToString()
            => string.Format((RequestOptions.JsonAuto | RequestOptions.Scalar).SnakeTemplate(),Payload);
        public string GetCommand(RequestOptions format = RequestOptions.JsonAuto)
            => ToString();
    }
}

using MediatR;
using System.Linq.Expressions;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore.Collections;

namespace MissCore.Bot
{
    public record ReadUnit : UnitRequest, IUnitRequest
    {
        public ReadUnit()
        {
            Options = RequestOptions.JsonAuto | RequestOptions.Scalar;
        }
        public IUnitRequest Read<TEntity>() where TEntity : class
            => this with { Payload = string.Format(Payload, Unit<TEntity>.Key) };        
        public override string ToString()
            => Payload  + Options.Format();
        public override string GetCommand()
            => ToString();
    }
}

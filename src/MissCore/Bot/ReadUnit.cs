using MissBot.Abstractions.Actions;
using MissCore.Data;
using MissCore.DataAccess;

namespace MissCore.Bot
{
    public record ReadUnit : UnitRequest
    {
        public ReadUnit()
        {
            Options = RequestOptions.JsonAuto | RequestOptions.Scalar;
        }

        public IUnitRequest Read<TEntity>() where TEntity : class
            => this with { Extension = string.Format(Extension, Unit<TEntity>.Key) };
        public override string ToString()
            => Extension  + Options.Format();
        public override string GetCommand()
            => ToString();
    }
}

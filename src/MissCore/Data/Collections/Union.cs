using System.Text.Json.Nodes;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using System.Linq;

namespace MissCore.Data.Collections
{
    [JsonArray]
    public class Union<TUnit> : Unit<TUnit>.Collection, IList<TUnit>, IMetaCollection where TUnit : IBotUnit
    {
        public IEnumerable<IMetaValue> KeyValues { get; }

        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Add(obj);

        public IEnumerable<TSubUnit> BringTo<TSubUnit>() where TSubUnit : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMetaValue> GetValues()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TSub> SupplyTo<TSub>() where TSub : class
        {
            throw new NotImplementedException();
        }
    }
}

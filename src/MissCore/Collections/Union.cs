using System.Text.Json.Nodes;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using System.Linq;

namespace MissCore.Collections
{       
    [JsonArray]
    public class Union<TUnit> : Unit<TUnit>.Collection, IList<TUnit>, IMetaCollection where TUnit : IBotUnit
    {
        public IEnumerable<KeyValuePair<string, object>> KeyValues { get; }

        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Add(obj);

        public IEnumerable<TUnit1> BringTo<TUnit1>() where TUnit1 : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity>(Predicate<TEntity> criteria = default) where TEntity :class
        {
            return null;// this.Where(w => w.Entity == BotUnit<TEntity>.Key);
        }

        public IEnumerator<TEntity> GetEnumerator<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            throw new NotImplementedException();
        }

        public TSub[] SupplyTo<TSub>() where TSub : class
        {
            throw new NotImplementedException();
        }

  

        IEnumerable<TSub> IMetaCollection.SupplyTo<TSub>()
        {
            throw new NotImplementedException();
        }
    }

    
}

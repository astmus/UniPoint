using System.Collections;
using System.Linq;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{
    public class MetaCollection<TUnit> : IEnumerable<TUnit>, IMetaCollection<TUnit> where TUnit : class
    {
        private readonly IEnumerable<JObject> tokens;
        public MetaData<TUnit> Metadata { get; protected set; }
        public IEnumerable<KeyValuePair<string, object>> KeyValues { get; }
        public readonly static MetaCollection<TUnit> Empty = new MetaCollection<TUnit>(Enumerable.Empty<TUnit>());
        public MetaCollection(IEnumerable<TUnit> items)
        {
            tokens = items.Select(s => JObject.FromObject(s));
            Metadata = MetaData<TUnit>.Parse(items.FirstOrDefault());
        }

        public MetaCollection(IEnumerable<JToken> dataTokens)
        {
            tokens = dataTokens.Select(s => JObject.FromObject(s));
            Metadata = new MetaData<TUnit>().Parse(tokens.FirstOrDefault());
        }
        public MetaCollection(JArray items)
        {
            tokens = items.Select(s => JObject.FromObject(s));
            Metadata = new MetaData<TUnit>().Parse(tokens.FirstOrDefault());
        }

        public virtual IEnumerable<TSub> BringTo<TSub>() where TSub : class
        {
            foreach (var token in tokens)
            {
                var res = Bring<TSub>(token);
                yield return res;
            }
        }

        public IEnumerator<TUnit> GetEnumerator()
        {
            foreach (var token in tokens)
            {
                var result = Bring<TUnit>(token);
                yield return result;
            }
        }

        private TRes Bring<TRes>(JObject token) where TRes:class
        {
            Metadata.SetContainer(token);
            var result = Metadata.Bring<TRes>();
            if (result is BaseUnit unit)
                unit.Meta = MetaData<TUnit>.FromRaw(token, Metadata);
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();


        public IEnumerable<TSub> SupplyTo<TSub>() where TSub : class
        {
            foreach (var token in tokens)
            {
                var result = Bring<TSub>(token);
                yield return result;
            }
        }

        public IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity : class
        {
            foreach (var token in tokens)
            {
                var result = token.ToObject<TEntity>();
                if (result is BaseUnit unit)
                    unit.Meta = MetaData<TUnit>.FromRaw(token, Metadata);
                yield return result;
            }
        }

        public IEnumerable<IMetaValue> GetValues()
        {
            foreach (var token in tokens)
            {
                Metadata.SetContainer(token);
                foreach (var item in Metadata.Keys)
                    yield return new MetaValue(item, Metadata.GetValue(item));
            }
        }
    }

    public class MetaCollection : IMetaCollection
    {
        private readonly IEnumerable<JToken> tokens;
        public MetaData Metadata { get; protected set; }
        public IEnumerable<IMetaValue> GetValues()
        {
            foreach (var token in tokens)
            {
                Metadata.SetContainer(token);
                foreach (var item in Metadata.Keys)
                    yield return new MetaValue(item, Metadata.GetValue(item));
            }
        }

        public MetaCollection(IEnumerable<JToken> items)
        {
            Metadata = MetaData.Parse(items.FirstOrDefault());
            tokens = items;
        }
        public MetaCollection(JArray items)
        {
            tokens = items.Select(s => JObject.FromObject(s));
            Metadata = MetaData.Parse(tokens.FirstOrDefault());
        }

        public IEnumerable<TSub> SupplyTo<TSub>() where TSub : class
        {
            foreach (var token in tokens)
            {                
                Metadata.SetContainer(token);
                var result = Metadata.Bring<TSub>();
                if (result is BaseUnit unit)
                    unit.Meta = MetaData.Parse(token);
                yield return result;
            }
        }

        public IEnumerable<TUnit> BringTo<TUnit>() where TUnit : class
        {
            foreach (var item in tokens)
            {
                Metadata.SetContainer(item);
                yield return Metadata.Bring<TUnit>();
            }
        }
        public IEnumerable<string> GetEnumerator()
        {
            foreach (var item in tokens)
            {
                Metadata.SetContainer(item);
                yield return Metadata.StringValue;
            }
        }

    }

}

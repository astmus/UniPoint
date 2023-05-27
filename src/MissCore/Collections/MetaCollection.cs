using System.Collections;
using System.Linq;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{


    public class MetaCollection<TUnit> : IEnumerable<TUnit>, IMetaCollection<TUnit> where TUnit : class
    {
        private readonly IEnumerable<JObject> tokens;
        public MetaData<TUnit> Metadata { get; protected set; }
        public IEnumerable<KeyValuePair<string, object>> KeyValues { get; }

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
                Metadata.SetContainer(token);
                var result = Metadata.Bring<TSub>();
                if (result is UnitBase unit)
                    unit.Meta = MetaData<TUnit>.FromRaw(token, Metadata);
                yield return result;
            }
        }

        public IEnumerator<TUnit> GetEnumerator()
        {
            foreach (var token in tokens)
            {
                Metadata.SetContainer(token);
                yield return Metadata.Bring<TUnit>();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public IEnumerable<TEntity> Get<TEntity>(Predicate<TEntity> criteria = default) where TEntity : class
        {
            return BringTo<TEntity>();
        }

        public IEnumerable<TSub> SupplyTo<TSub>() where TSub : class
        => tokens.Select(token =>
        {
            Metadata.SetContainer(token);
            var result = Metadata.Bring<TSub>();
            if (result is UnitBase unit)
                unit.Meta = MetaData<TUnit>.FromRaw(token, Metadata);
            return result;
        }).ToArray();



        public IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity : class,IUnit<TUnit>
        {
            foreach (var token in tokens)
            {
                var result = token.ToObject<TEntity>();
                if (result is UnitBase unit)
                    unit.Meta = MetaData<TUnit>.FromRaw(token, Metadata);
                yield return result;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            foreach (var token in tokens)
            {
                Metadata.SetContainer(token);
                foreach (var item in Metadata.Keys)
                    yield return KeyValuePair.Create(item, (object)Metadata.GetValue(item));
            }
        }
    }

    public class MetaCollection :  IMetaCollection
    {
        private readonly IEnumerable<JToken> tokens;
        public MetaData Metadata { get; protected set; }
        public IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            foreach (var token in tokens)
            {
                Metadata.SetContainer(token);
                foreach (var item in Metadata.Keys)
                {
                    //foreach (var prop in token.Children<JProperty>())


                    yield return KeyValuePair.Create(item, Metadata.GetValue(item));
                }
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
            => tokens.Select(token =>
                {
                    Metadata.SetContainer(token);
                    var result = Metadata.Bring<TSub>();
                    if (result is UnitBase unit)
                        unit.Meta = MetaData.Parse(token);
                    return result;
                }).ToArray();

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

        public IEnumerable<TEntity> Get<TEntity>(Predicate<TEntity> criteria = null) where TEntity : class
        {
            return BringTo<TEntity>();
        }



        public IEnumerator<TEntity> GetEnumerator<TEntity>() where TEntity : class
        {
            foreach (var item in Metadata.Keys.Select(key => Metadata.BringChild<TEntity>(key)))
            {
                //Metadata.first = item;
                yield return item;
            }
        }

       
    }

}

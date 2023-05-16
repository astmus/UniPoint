using System.Collections;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{
    public class MetaCollection : IEnumerable<string>
    {
        private readonly IEnumerable<JToken> items;
        public MetaData Metadata { get; protected set; }
        public MetaCollection(IEnumerable<JToken> tokens)
        {
            Metadata = MetaData.Parse(tokens.FirstOrDefault());
            items = tokens;
        }

        public virtual TSub[] SupplyTo<TSub>() where TSub : Unit
            => items.Select(item =>
                {
                    Metadata.first = item;
                    var result = Metadata.Bring<TSub>();
                    result.Meta = MetaData.Parse(item);
                    return result;
                }).ToArray();

        public IEnumerable<TUnit> Bring<TUnit>() where TUnit : class
        {
            foreach (var item in items)
            {
                Metadata.first = item;
                yield return Metadata.Bring<TUnit>();
            }
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var item in items)
            {
                Metadata.first = item;
                yield return Metadata.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

}

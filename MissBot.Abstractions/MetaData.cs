//using System.Collections;
//using System.Collections.Specialized;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using MissBot.Abstractions.Entities;
//using Newtonsoft.Json.Linq;

//namespace MissBot.Abstractions
//{
//    public class MetaCollection : IEnumerable<string>
//    {
//        private readonly IEnumerable<JToken> items;
//        public MetaData Metadata { get; protected set; }
//        public MetaCollection(IEnumerable<JToken> tokens)
//        {
//            Metadata = MetaData.Parse(tokens.FirstOrDefault());
//            items = tokens;
//        }

//        public virtual TSub[] LandOn<TSub>() where TSub : Unit
//            =>  items.Select(item =>
//                {                                    
//                    Metadata.Pointer = item;
//                    var result = Metadata.Bring<TSub>();
//                    result.Meta = new MetaData(item);
//                    return result;
//                }).ToArray();                   
        
//        public IEnumerable<TUnit> Bring<TUnit>() where TUnit : class
//        {
//            foreach (var item in items)
//            {
//                Metadata.Pointer = item;
//                yield return Metadata.Bring<TUnit>();
//            }
//        }
//        public IEnumerator<string> GetEnumerator()
//        {
//            foreach (var item in items)
//            {
//                Metadata.Pointer = item;
//                yield return Metadata.Value;
//            }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//            => GetEnumerator();
//    }

//    public class MetaData<T> : MetaData where T : JToken
//    {
//        public MetaData(T data) : base(data)
//        {
//        }   
       
//    }

//    public interface IMetaUnit
//    {
//        void Operation();
//    }
//    public record MetaUnit : IMetaUnit
//    {
//        public virtual void Operation()
//        {
//            throw new NotImplementedException();
//        }
//    }
//    public record MetaDecorator : MetaUnit
//    {
//        protected IMetaUnit component;
//        public void SetComponent(IMetaUnit component)
//        {
//            this.component = component;
//        }
//        public override void Operation()
//        {
//            if (component != null)
//            {
//                component.Operation();
//            }
//        }
//    }
//    public class MetaData : ListDictionary
//    {
//        internal JToken Pointer;
//        public MetaData()
//        {
//        }
//        public MetaData(JToken data = default)
//        {
//            if (data == default) return;
//            Pointer = data; 
//            ParseTokens(Pointer);
//        }
//        public string Value
//            => string.Join(" ", this.Values.Cast<string>().Select(s => string.Format("<b>{0}</b>", Pointer.SelectToken(s).ToString())));

//        public static MetaData Parse<TData>(TData token)
//        {
//            return new MetaData(JToken.FromObject(token));
//        }
//        protected void ParseTokens(JToken containerToken)
//        {
//            if (containerToken.Type == JTokenType.Object)
//            {
//                foreach (JProperty child in containerToken.Children<JProperty>())
//                {
//                    Add(child.Name, child.Path);
//                    ParseTokens(child.Value);
//                }
//            }
//            else if (containerToken.Type == JTokenType.Array)
//            {
//                foreach (JToken child in containerToken.Children())
//                    ParseTokens(child);
//            }
//        }
        
//        public virtual TUnit Bring<TUnit>() where TUnit: class
//            => Pointer.ToObject<TUnit>();
//        public virtual TChild BringChild<TChild>(string childPath = default) where TChild : class
//        {
//            if (this[childPath ?? typeof(TChild).Name] is string path)
//                return Pointer.SelectToken(path).ToObject<TChild>();
//            else return default;
//        }
//        public object GetByName([CallerMemberName] string name = default)
//        {
//            return this[name];
//        }   
//    }

//}

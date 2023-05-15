using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{

    public class MetaData<T> : MetaData where T : JToken
    {
        public MetaData(T data) : base(data)
        {
        }

    }
    public readonly record struct MetaItem(string name, string value) : IFormattable
    {
        public string ToString(string format, IFormatProvider formatProvider)
            => $"{name}: {value}";
    }

    public interface IMetaUnit
    {
        void Operation();
    }
    public record MetaUnit : IMetaUnit
    {
        public virtual void Operation()
        {
            throw new NotImplementedException();
        }
    }
    public record MetaDecorator : MetaUnit
    {
        protected IMetaUnit component;
        public void SetComponent(IMetaUnit component)
        {
            this.component = component;
        }
        public override void Operation()
        {
            if (component != null)
            {
                component.Operation();
            }
        }
    }
    public class MetaData : ListDictionary, IMetaData
    {
        internal JToken Pointer;
        public MetaData()
        {
        }
        public MetaData(JToken data, bool shrinkPath = false)
        {
            Pointer = data; //JToken.FromObject(data);
            ParseTokens(Pointer);
        }
        public string GetRecord(int index, string format)
        {
            var token = this[index];
            return new MetaItem(token.Path, token.ToString()).ToString(format, null);
        }

        public string Value
            => string.Join(" ", Items.Select(s => string.Format(new MetaItem(Pointer.Path, Pointer.SelectToken(s).ToString()).ToString("<b>{0}</b>", null))));
        protected IEnumerable<string> Items
            => Values.Cast<string>();

        protected JToken this[int index]
            => Pointer.SelectToken(Items.ElementAt(index));

        public static MetaData Parse<TData>(TData token)
            => new MetaData(JToken.FromObject(token));

        public virtual TUnit Bring<TUnit>() where TUnit : class
            => Pointer.ToObject<TUnit>();

        public virtual TChild BringChild<TChild>(string childPath = default) where TChild : class
        {
            if (this[childPath ?? typeof(TChild).Name] is string path)
                return Pointer.SelectToken(path).ToObject<TChild>();
            else return default;
        }
        public object GetByName([CallerMemberName] string name = default)
        {
            return this[name];
        }
        protected void ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (var child in containerToken.Children<JProperty>())
                {
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (var child in containerToken.Children())
                    ParseTokens(child);
            }
        }
    }

}

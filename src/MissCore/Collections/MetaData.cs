using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{

    public readonly record struct MetaItem(string name, string value) : IMetaItem
    {
        public string Format(IMetaItem.Formats format)
        {
            uint bvalue = 0x01;
            uint bits = Convert.ToUInt32(format);
            var strFormat = String.Empty;
            while (bvalue <= bits)
            {
                var b = bvalue & bits;
                if ((bvalue & bits) > 0)
                    strFormat += GetFormat((IMetaItem.Formats)b);
                bvalue <<= 1;
            }
            return string.Format(strFormat, name, value);
        }
        static string GetFormat(IMetaItem.Formats format) => format switch
        {
            IMetaItem.Formats.NewLine => "\n",
            IMetaItem.Formats.B => "<b>{0} {1}</b>",
            IMetaItem.Formats.I => "<i>{0} {1}</i>",
            IMetaItem.Formats.Code => "<code>{0} {1}</code>",
            IMetaItem.Formats.Strike => "<s>{0} {1}</s>",
            IMetaItem.Formats.Under => "<u>{0} {1}</u>",
            IMetaItem.Formats.Pre => "<pre>{0} {1}</pre>",
            IMetaItem.Formats.Link => "<a href=\"{0}\">{1}</a>",
            IMetaItem.Formats.BSection => "<b>{0}</b>: {1}",
            IMetaItem.Formats.Percent => " % ",
            IMetaItem.Formats.Equal => " = ",
            IMetaItem.Formats.Section => "{0}: {1}",
            _ => String.Empty
        };
        public override string ToString()
        {
            return ToString(default, default);
        }
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
    public class MetaData<TUnit> : MetaData, IMetaData
    {
        //public IMetaItem GetItem(int index)
        //{
        //    var token = this[index];
        //    return new MetaItem(token.Path, token.ToString());
        //}
        public static MetaData<TUnit> FromRaw(JObject container, MetaData<TUnit> clone)
        {
            var data = clone.MemberwiseClone() as MetaData<TUnit>;
            data.first = container;
            return data;
        }
        public static new MetaData<TData> Parse<TData>(TData data)
            => new MetaData<TData>().Parse(JObject.FromObject(data));
        //public string Value
        //    => string.Join(" ", Items.Select(s => string.Format(new MetaItem(first.Path, first.SelectToken(s).ToString()).ToString("<b>{0}</b>", null))));

        //protected IEnumerable<string> Items
        //    => Values.Cast<string>();

        //protected JToken this[int index]
        //    => first.SelectToken(Items.ElementAt(index));

        //public virtual TSubUnit Bring<TSubUnit>() where TSubUnit : class
        //    => first.ToObject<TSubUnit>();

        //public virtual TChild BringChild<TChild>(string childPath = default) where TChild : class
        //{
        //    if (this[childPath ?? typeof(TChild).Name] is string path)
        //        return first.SelectToken(path).ToObject<TChild>();
        //    else return default;
        //}
        public MetaData<TUnit> Parse(JObject containerToken)
        {
            first = containerToken;
            return ParseTokens(containerToken);
        }

        protected override MetaData<TUnit> ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
                foreach (var child in containerToken.Children<JProperty>())
                {
                    if (!Contains(child.Name))
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
                foreach (var children in containerToken.Children())
                    foreach (var child in children.Children<JProperty>())
                    {
                        Add(child.Path, child.Path);
                        ParseTokens(child.Value);
                    }

            return this;
        }
    }
    public class MetaData : ListDictionary, IMetaData
    {
        protected JToken first;

        public IMetaItem GetItem(int index)
        {
            var token = this[index];
            return token != null ?  new MetaItem(token.Path, token.ToString()) : null;
        }

        //public string Value
        //    => string.Join(" ", Values.Select(s => new MetaItem(s, first.SelectToken(s).ToString()).ToString()));

        public new IEnumerable<string> Values
            => base.Values.Cast<string>();

        public new IEnumerable<string> Keys
            => base.Keys.Cast<string>();

        public string StringValue
            => string.Join(" ", Keys.Select(key => $"{key}: {GetValue(key)}"));

        protected JToken this[int index]
        {
            get
            {
                if (Values.ElementAtOrDefault(index) is string path)
                    return first.SelectToken(path);
                else return default;
            }
        }

        public static MetaData Parse<TData>(TData data)
            => new MetaData().ParseTokens(JToken.FromObject(data));

        public virtual TUnit Bring<TUnit>() where TUnit : class
            => first.ToObject<TUnit>();

        public virtual TChild BringChild<TChild>(string childPath = default) where TChild : class
        {
            if (this[childPath ?? typeof(TChild).Name] is string path)
                return first.SelectToken(path).ToObject<TChild>();
            else return default;
        }

        public object GetByName([CallerMemberName] string name = default)
        {
            return this[name];
        }
        protected virtual MetaData ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
                foreach (var child in containerToken.Children<JProperty>())
                {
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
                foreach (var child in containerToken.Children())
                    ParseTokens(child);

            first = containerToken;
            return this;
        }

        public object GetValue(string key)
        {
            if (this[key] is string path && first.SelectToken(path) is JValue prop)
                return prop.Value;
            else
                return default;
        }

        public void SetItem<TItem>(string name, TItem item)
        {
            if (first is JObject obj)
            {
                if (obj.SelectToken(name) is JToken token)
                    token.Replace(new JProperty(name, item));
                else
                {
                    Add(name, name);
                    obj.Add(new JProperty(name, item));
                }
            }
        }

        public void SetContainer<TContainer>(TContainer container) where TContainer : JToken
        {
            first = container;
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{

    //public readonly record struct MetaItem(string name, string value) : IMetaItem
    //{
    //    public string Format(IMetaItem.Formats format)
    //    {
    //        uint bvalue = 0x01;
    //        uint bits = Convert.ToUInt32(format);
    //        var strFormat = String.Empty;
    //        while (bvalue <= bits)
    //        {
    //            var b = bvalue & bits;
    //            if ((bvalue & bits) > 0)
    //                strFormat += GetFormat((IMetaItem.Formats)b);
    //            bvalue <<= 1;
    //        }
    //        return string.Format(strFormat, name, value);
    //    }
    //    static string GetFormat(IMetaItem.Formats format) => format switch
    //    {
    //        IMetaItem.Formats.NewLine => "\n",
    //        IMetaItem.Formats.B => "<b>{0} {1}</b>",
    //        IMetaItem.Formats.I => "<i>{0} {1}</i>",
    //        IMetaItem.Formats.Code => "<code>{0} {1}</code>",
    //        IMetaItem.Formats.Strike => "<s>{0} {1}</s>",
    //        IMetaItem.Formats.Under => "<u>{0} {1}</u>",
    //        IMetaItem.Formats.Pre => "<pre>{0} {1}</pre>",
    //        IMetaItem.Formats.Link => "<a href=\"{0}\">{1}</a>",
    //        IMetaItem.Formats.BSection => "<b>{0}</b>: {1}",
    //        IMetaItem.Formats.Percent => " % ",
    //        IMetaItem.Formats.Equal => " = ",
    //        IMetaItem.Formats.Section => "{0}: {1}",
    //        _ => String.Empty
    //    };
    //    public override string ToString()
    //    {
    //        return ToString(default, default);
    //    }
    //    public string ToString(string format, IFormatProvider formatProvider)
    //        => $"{name}: {value}";
    //}
    // Provides the Create factory method for KeyValuePair<TKey, TValue>.
    
    public readonly record struct MetaItem(JProperty token) : IMetaItem
    {
        public string UnitName
            => token switch
            {
                { Parent:{ } } v when v.Parent is JProperty pa => pa.Name,
                _ => token.Name
            };
        public object UnitValue
            => token.Value is JValue val ? val.Value : null;
        public override string ToString()
            => Serialize();
        public string Serialize()
            => $"{UnitName}: {UnitValue}";
    }

    public record struct MetaItemDecorator : IMetaItem
    {
        IMetaItem component;

        public string UnitName
            => $"<b>{component.UnitName}:</b>";
        public object UnitValue
            => component.UnitValue;

        public void SetComponent(ref IMetaItem component)
        {
            this.component = component;
        }
        public  string Serialize()
        {            
            return $"{UnitName} {UnitValue}";
        }
    }
    public class MetaData<TUnit> : MetaData, IMetaData
    {

        public static MetaData<TUnit> FromRaw(JObject container, MetaData<TUnit> clone)
        {
            var data = clone.MemberwiseClone() as MetaData<TUnit>;
            data.root = container;
            return data;
        }
        public static new MetaData<TData> Parse<TData>(TData data)
            => data != null ? new MetaData<TData>().Parse(JObject.FromObject(data)) : null;
     
        public MetaData<TUnit> Parse(JObject containerToken)
        {
            if (containerToken == null)
                return null;
            root = containerToken;
            return ParseTokens(containerToken);
        }

        protected override MetaData<TUnit> ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
                foreach (var child in containerToken.Children<JProperty>())
                {
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
                foreach (var children in containerToken.Children())
                    foreach (var child in children.Children<JProperty>())
                    {
                        Add(child.Name, child.Path);
                        ParseTokens(child.Value);
                    }

            return this;
        }
    }

    public class MetaData : ListDictionary, IMetaData
    {
        protected internal JToken root;
        public MetaData() : base(StringComparer.OrdinalIgnoreCase)
        {

        }
        public IMetaItem GetItem(string key) 
        {
            if (this[key] is string path && root.SelectToken(path) is JValue val && val.Parent is JProperty prop)
                return new MetaItem(prop);
            return null;
        }

        public IMetaItem GetItem(int index) => this[index] switch
        {
            JValue value when value.Parent is JProperty prop => new MetaItem(prop),
            JProperty prop when prop.Value is JValue value => new MetaItem(prop),
            _ => null
        };

        public new IEnumerable<string> Values
            => base.Values.Cast<string>();

        public new IEnumerable<string> Keys
            => base.Keys.Cast<string>();

        public string StringValue
            => string.Join(" ", Keys.Select(key => $"{key}: {GetValue(key)}"));

        public IEnumerable<IMetaItem> Items
        {
            get
            {
                for (byte i = 0; i < Count; i++)                
                    yield return GetItem(i);                
            }
        }

        protected JToken this[int index]
        {
            get
            {
                if (Values.ElementAtOrDefault(index) is string path)
                    return root.SelectToken(path);
                else return default;
            }
        }

        public static MetaData Parse<TData>(TData data)
            => new MetaData().ParseTokens(JToken.FromObject(data));

        public virtual TUnit Bring<TUnit>() where TUnit : class
            => root.ToObject<TUnit>();

        public virtual TChild BringChild<TChild>(string childPath = default) where TChild : class
        {
            if (this[childPath ?? typeof(TChild).Name] is string path)
                return root.SelectToken(path).ToObject<TChild>();
            else return default;
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
                foreach (var children in containerToken.Children())
                    foreach (var child in children.Children<JProperty>())
                    {
                        Add(child.Path, child.Path);
                        ParseTokens(child.Value);
                    };

            root = containerToken;
            return this;
        }

        public object GetValue(string key)
        {
            if (this[key] is string path && root.SelectToken(path) is JValue prop)
                return prop.Value;
            else
                return default;
        }

        public void SetItem<TItem>(string name, TItem item)
        {
            if (root is JObject obj)
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
            root = container;
        }
    }
}

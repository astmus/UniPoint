using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{

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
                    //ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
                foreach (var children in containerToken.Children())
                    foreach (var child in children.Children<JProperty>())
                    {
                        Add(child.Name, child.Path);
                        //ParseTokens(child.Value);
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
        public IUnitItem GetItem(string key)
        {
            if (this[key] is string path && root.SelectToken(path) is JValue val)
                return new UnitItem(val.Parent as JProperty);
            return null;
        }

        public IUnitItem GetItem(int index) => this[index] switch
        {
            JValue value when value.Parent is JProperty prop => new UnitItem(prop),
            JProperty prop when prop.Value is JValue value => new UnitItem(prop),
            _ => null
        };

        public new IEnumerable<string> Values
            => base.Values.Cast<string>();

        public new IEnumerable<string> Keys
            => base.Keys.Cast<string>();

        public string StringValue
            => string.Join(" ", Keys.Select(key => $"{key}: {GetValue(key)}"));

        public IEnumerable<IUnitItem> Items
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
            => Activator.CreateInstance<MetaData>().ParseTokens(JToken.FromObject(data));

        public virtual TUnit Bring<TUnit>() where TUnit : class
            => root.ToObject<TUnit>();

        public virtual TChild BringChild<TChild>(string childPath = default) where TChild : class
        {
            if (this[childPath ?? typeof(TChild).Name] is string path)
                return root.SelectToken(path).ToObject<TChild>();
            else return default;
        }

        protected virtual MetaData ParseTokens(JToken token)
        {
            if (token.Type == JTokenType.Object)
                foreach (var child in token.Children<JProperty>())
                {
#if DEBUG
                    Console.WriteLine($"{child.Name}:{child.Path}");
#endif
                    //if (!Contains(child.Name))
                        Add(child.Name, child.Path);
                    //else
                    //    Add(child.Path, child.Path);
                    //ParseTokens(child.Value);
                }
            else if (token.Type == JTokenType.Array)
                foreach (var children in token.Children())
                    foreach (var child in children.Children<JProperty>())
                    {
                        Add(child.Path, child.Path);
                        //ParseTokens(child.Value);
                    };

            root = token;
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

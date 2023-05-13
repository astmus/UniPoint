using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public delegate IEnumerable<string> FieldNamesSelector<TUnit>(TUnit entity);
    public delegate (string field, string value) WhereSelector<TUnit>(TUnit entity);
    public delegate void LoadSelector<TUnit>(TUnit entity);
    public class DataMap : NameValueCollection
    {
        JToken token;
        public DataMap(object data)
        {
            token = JToken.FromObject(data);
            ParseTokens(token);
        }
        public void Parse<TData>(TData value)
            => ParseTokens(JToken.FromObject(value));

        public TD Read<TD>(string name)
        {
            if (this[name] is string path && token.SelectToken(path) is JToken value)
                return value.ToObject<TD>();
            else return default;
         }


        private void ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    this.Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    ParseTokens(child);
                }
            }
        }
    }
    [JsonArray]
    public record Union<TUnit> : Unit<TUnit>.Union, IList<TUnit>
    {
        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Content.Add(obj);
    }

    public interface IUnitCollection
    {
        string ToString();
    }

    [JsonObject]
    public record Unit<TEntity>(TEntity Entity = default) : ValueUnit//, //<TEntity>
    {
        public static readonly TEntity Sample = Activator.CreateInstance<TEntity>();
        internal static readonly string UnitName = typeof(TEntity).Name;
        public static DataMap Parse(TEntity entity)
            => new DataMap(entity);
        public record Collection : Union<TEntity>, IUnitCollection;
        // public class ContentUnit<TContent> where TContent : Unit<TEntity> { }
        //public virtual ContentUnit<TEntity> Content { get; set; }
        public static string Stringify(string[] items)
            => string.Join('\t', from s in items
                                 where s.Length > 2 && !s.EndsWith("= ")
                                 select s);
        public static string ParseTyped(object value)
            => Stringify(value.ToString().Split('{', ':', ',', '}'));

        #region Union
        [JsonArray]
        public record Union : Unit, IList<TEntity>
        {
            public Union()
            {

            }
            public Union(IEnumerable<TEntity> units = default)
            {
                if (units != null)
                    Content.AddRange(units);
            }
            List<TEntity> union;
            protected List<TEntity> Content
                => union ?? (union = new List<TEntity>());

            public int Count => ((ICollection<TEntity>)Content).Count;

            public bool IsReadOnly => ((ICollection<TEntity>)Content).IsReadOnly;

            public TEntity this[int index] { get => ((IList<TEntity>)Content)[index]; set => ((IList<TEntity>)Content)[index] = value; }

            public int IndexOf(TEntity item)
            {
                return ((IList<TEntity>)Content).IndexOf(item);
            }

            public void Insert(int index, TEntity item)
            {
                ((IList<TEntity>)Content).Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                Content?.RemoveAt(index);
            }

            public void Clear()
            {
                ((ICollection<TEntity>)Content).Clear();
            }

            public bool Contains(TEntity item)
            {
                return ((ICollection<TEntity>)Content).Contains(item);
            }

            public void CopyTo(TEntity[] array, int arrayIndex)
            {
                ((ICollection<TEntity>)Content).CopyTo(array, arrayIndex);
            }

            public bool Remove(TEntity item)
            {
                return ((ICollection<TEntity>)Content).Remove(item);
            }

            public IEnumerator<TEntity> GetEnumerator()
            {
                return Content?.GetEnumerator() ?? Enumerable.Empty<TEntity>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            //public static implicit operator List<TUnit>(Union unit)
            //    => unit.Units;
            //public static implicit operator Union(List<TUnit> units)
            //    => new Union(units);

            public Union Add(params TEntity[] units)
            {
                Content.AddRange(units);
                return this;
            }

            public void Add(TEntity item)
            {
                ((ICollection<TEntity>)Content).Add(item);
            }


        }
        #endregion

        #region IList

        // IEnumerator IEnumerable.GetEnumerator()
        // =>
        //     Content?.GetEnumerator();



        // public int IndexOf(TEntity item)
        // => Content?.IndexOf(item) ?? -1;


        // public void Insert(int index, TEntity item)
        // =>
        //     Content.Insert(index, item);


        // public void RemoveAt(int index)
        // =>
        //     Content.RemoveAt(index);


        // public void Add(TEntity item)
        // =>
        //     Content.Add(item);


        // public void Clear()
        //     => Content?.Clear();


        // public bool Contains(TEntity item)
        //=> Content?.Contains(item)  == true;


        // public void CopyTo(TEntity[] array, int arrayIndex)
        // =>
        //     Content.CopyTo(array, arrayIndex);


        // public bool Remove(TEntity item)
        // =>
        //   Content.Remove(item);


        // public IEnumerator<TEntity> GetEnumerator()
        //     => Content?.GetEnumerator();


        // public int Count => Content?.Count ?? 0;

        // public bool IsReadOnly => false;

        // public TEntity this[int index] { get => Content[index]; set => Content[index] = value; }
        #endregion
    }
}

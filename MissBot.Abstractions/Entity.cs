using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;


namespace MissBot.Abstractions
{
    [JsonArray]
    public record Union<TUnit> : BotEntity<TUnit>.Union , IList<TUnit>
    {
        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Content.Add(obj);
    }
    public record BotUnion : ValueUnit, IList<ValueUnit>
    {
        public BotUnion(IEnumerable<ValueUnit> units = default)
        {
            if (units != null)
                Union.AddRange(units);
        }
        List<ValueUnit> union;
        protected List<ValueUnit> Union
            => union ?? (union = new List<ValueUnit>());
        public int Count => Union?.Count ?? 0;
        public bool IsReadOnly
            => false;

        ValueUnit IList<ValueUnit>.this[int index] { get => ((IList<ValueUnit>)Union)[index]; set => ((IList<ValueUnit>)Union)[index] = value; }
        public BotUnion this[int index] { get => ((IList<BotUnion>)Union)[index]; set => ((IList<BotUnion>)Union)[index] = value; }

        public static implicit operator List<ValueUnit>(BotUnion unit)
            => unit.Union;
        public static implicit operator BotUnion(List<ValueUnit> units)
            => new BotUnion(units);
        public void Add(ValueUnit obj)
            => this.Union.Add(obj);
        public BotUnion Add(params ValueUnit[] units)
        {
            Union.AddRange(units);
            return this;
        }


        public int IndexOf(ValueUnit item)
        {
            return ((IList<ValueUnit>)Union).IndexOf(item);
        }

        public void Insert(int index, ValueUnit item)
        {
            ((IList<ValueUnit>)Union).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Union?.RemoveAt(index);
        }

        public void Clear()
        {
            ((ICollection<ValueUnit>)Union).Clear();
        }

        public bool Contains(ValueUnit item)
        {
            return ((ICollection<ValueUnit>)Union).Contains(item);
        }

        public void CopyTo(ValueUnit[] array, int arrayIndex)
        {
            ((ICollection<ValueUnit>)Union).CopyTo(array, arrayIndex);
        }

        public bool Remove(ValueUnit item)
        {
            return ((ICollection<ValueUnit>)Union).Remove(item);
        }

        public IEnumerator<ValueUnit> GetEnumerator()
        {
            return Union?.GetEnumerator() ?? Enumerable.Empty<ValueUnit>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }

    [JsonObject]
    public record Unit<TEntity> : ValueUnit, IFormattable, ICollection<TEntity>
    {
        public static readonly TEntity Sample = Activator.CreateInstance<TEntity>();
        internal static string EntityName;
        static readonly Unit<TEntity>.MetaUnit _metaUnit;
        Union<TEntity> content;

        public override sealed bool IsSimpleUnit()
            => false;
        protected virtual void InvalidateMetaData(TEntity unit){ }

        static Unit()
        {        
            EntityName = typeof(TEntity).Name;
        }
        public Union<TEntity> Content
        {
            get =>
                content ?? (content = new Union<TEntity>());
                set => content = value;
        }
        public Unit<TEntity>.MetaUnit MetaData
            => Meta with { Content = EntityName, Data = Meta.Data ?? GetMetaData() };

        public static readonly Empty EmptyContent = new Empty();
        public record Empty(string Id = "0", string Text = "Empty", string Title = "Not found") : Unit(Id, Text,Title)
        {
            public TEntity[] Content { get; set; } = { Unit<TEntity>.Sample };
        }


        public static readonly Unit<TEntity> Instance
            = new Unit<TEntity>();
        public static readonly Unit<TEntity>.MetaUnit Meta
            = _metaUnit ??= new MetaUnit(EntityName, ValueUnit.Parse(Sample));

        public object this[string key]
        {
            get => MetaInformation[key];
            set => MetaInformation.Set(value, key);
        }
        public override string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Join('\n', Content.Select(s => $"{EntityName}\n{Stringify(Convert.ToString(s).Split('{', ',', '}'))}"));

        }
           public static string Stringify(string[] items)
        => string.Join('\t', from s in items
                                            where s.Length > 2 && !s.EndsWith("= ")
                                            select s);
        public static string ParseTyped(object value)
            => Stringify(value.ToString().Split('{', ':',',', '}'));

           
        
        #region IList

        IEnumerator IEnumerable.GetEnumerator()
        =>
            Content.GetEnumerator();



        public int IndexOf(TEntity item)
        => Content.IndexOf(item);


        public void Insert(int index, TEntity item)
        =>
            Content.Insert(index, item);


        public void RemoveAt(int index)
        =>
            Content.RemoveAt(index);


        public void Add(TEntity item)
        =>
            Content.Add(item);


        public void Clear()
            => Content?.Clear();


        public bool Contains(TEntity item)
       => Content?.Contains(item) ?? false;


        public void CopyTo(TEntity[] array, int arrayIndex)
        =>
            Content.CopyTo(array, arrayIndex);


        public bool Remove(TEntity item)
        =>
          Content.Remove(item);


        public IEnumerator<TEntity> GetEnumerator()
            => Content.GetEnumerator();


        public int Count => Content?.Count ?? 0;

        public bool IsReadOnly =>false;

        public TEntity this[int index] { get => Content[index]; set => Content[index] = value; } 
        #endregion
    }
}

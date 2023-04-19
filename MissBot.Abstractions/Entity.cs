using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions
{
    public record BotUnion<TUnit> : BotEntity<Unit<TUnit>>.Union where TUnit : class
    {

    }
    [JsonObject]
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


    public record Unit<TEntity> : ValueUnit
    {
        public override sealed bool IsSimpleUnit()
            => false;
        static Unit()
        {
            var meta = Convert.ToString(Unit<TEntity>.Sample).Split('{', ',', '}').FirstOrDefault();
            // MetaUnit.EntityName ??= /*"##" +*/ meta;
            EntityName = meta;
            _metaUnit = InitMetaUnit();
        }
        TEntity entity;
        public TEntity Value { get => entity; init => entity = value; }
        internal static string EntityName;
        static MetaUnit _metaUnit;

        public MetaUnit MetaData
            => _metaUnit with { Data  = GetMetaData()  };

        static MetaUnit InitMetaUnit(MetaData meta = null)
            => new MetaUnit(EntityName, meta);
            
        //public override MetaData GetMetaData()
        //{
        //    //Meta.Set(Stringify((Value?.ToString() ?? this.ToString()).Split('{', ',', '}')), "Content");
        //    InvalidateMetaData(Value);
        //    return Meta;
        //}
        internal static string Stringify(string[] items)
        => string.Join(Environment.NewLine, from s in items
                                            where s.Length > 2 && !s.EndsWith("= ")
                                            select s);

        internal static string StringifyMeta(string[] items)
        => string.Join(" ", from s in items
                            where s.Length > 2 && !s.EndsWith("= ")
                            select s);

        protected virtual void InvalidateMetaData(TEntity unit)
        { }

        public static TEntity Sample
            => Instance.Value;
        public static readonly Unit<TEntity> Instance
            = new Unit<TEntity>() { Value = Activator.CreateInstance<TEntity>() };
        public static readonly Unit<TEntity>.MetaUnit Meta
            = new Unit<TEntity>.MetaUnit(EntityName, new MetaData());

        public object this[string key]
        {
            get => MetaInformation[key];
            set => MetaInformation.Set(value, key);
        }

    }
}

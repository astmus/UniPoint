using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MissBot.Abstractions
{
    public record BotUnion<TUnit> : BotEntity<TUnit>.Union, IList<TUnit>
    {
        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Units.Add(obj);        
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

    [JsonObject]
    public record Unit<TEntity> : ValueUnit
    {
        public override sealed bool IsSimpleUnit()
            => false;
        static Unit()
        {

            // MetaUnit.EntityName ??= /*"##" +*/ meta;
            EntityName = typeof(TEntity).Name;
            _metaUnit = InitMetaUnit();
        }
        public static readonly TEntity Sample = Activator.CreateInstance<TEntity>();
        internal static string EntityName;
        static MetaUnit _metaUnit;
        public BotUnion<TEntity> Content { get; set; }
        public MetaUnit MetaData
            => _metaUnit with { Data  = MetaInformation };

        static MetaUnit InitMetaUnit(MetaData meta = null)
            => new MetaUnit(EntityName, meta);
            

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

        public int IndexOf(TEntity item)
        =>Content.IndexOf(item);
        

        public void Insert(int index, TEntity item)
        =>
            Content.Insert(index, item);
        

        public void RemoveAt(int index)
        =>
            Content.RemoveAt(index);
        

        public void Add(TEntity item)
        {
            ((ICollection<TEntity>)Content).Add(item);
        }

        public void Clear()
            => Content?.Clear();
        

        public bool Contains(TEntity item)
        {
            return Content?.Contains(item) ?? false;
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
            return ((IEnumerable<TEntity>)Content).GetEnumerator();
        }

        public int Count => Content?.Count ?? 0;

        public bool IsReadOnly => Content?.IsReadOnly ?? false;

        public TEntity this[int index] { get => ((IList<TEntity>)Content)[index]; set => ((IList<TEntity>)Content)[index] = value; }

        public static readonly Unit<TEntity> Instance
            = new Unit<TEntity>();
        public static readonly Unit<TEntity>.MetaUnit Meta
            = InitMetaUnit(ValueUnit.Parse(Unit<TEntity>.Sample));

        public object this[string key]
        {
            get => MetaInformation[key];
            set => MetaInformation.Set(value, key);
        }

    }
}

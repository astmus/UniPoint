using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions
{
    public record BotUnion<TUnit> : BotEntity<TUnit>.Union where TUnit : class
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
        TEntity entity;
        public TEntity Value { get=>entity; init=> entity = value; }

        protected virtual void InvalidateEntityData(TEntity unit)
        { }
        public static TEntity Sample
            => Instance.Value;
        public static readonly Unit<TEntity> Instance
            = new Unit<TEntity>() { Value = Activator.CreateInstance<TEntity>() };
        public override MetaInfo Meta
        {
            get
            {
                InvalidateEntityData(Value);
                return MetaData;
            }
        }
    }

    public record ValueUnit
    {
        MetaInfo meta;
        protected MetaInfo MetaData
            => meta ?? (meta = new MetaInfo());
        public class MetaInfo : ConcurrentDictionary<string, object>
        {
            public T Get<T>([CallerMemberName] string name = default)
            {
                if (TryGetValue(name, out var r) && r is T value)
                    return value;
                return default(T);
            }

            public TAny FirstOrNull<TAny>(string name)
                => this.Where(x => x.Key == name && x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();


            public T Set<T>(T value, [CallerMemberName] string name = default)
            {
                AddOrUpdate<T>(name,
                    (k, w) => w,
                    (k, o, w) => this[k] = w,
                    value);
                return value;
            }
        }
        
        protected T Set<T>(T value, [CallerMemberName] string name = default)
            => MetaData.Set(value, name);
        protected T Get<T>([CallerMemberName] string name = default)
            => MetaData.Get<T>(name);
        public virtual MetaInfo Meta
        {
            get
            {
                
                return MetaData;
            }
        }

        
        
    }

    public static class BotEntity<TUnit>
    {
        public static TEntityUnit Instance<TEntityUnit>() where TEntityUnit : Unit<TUnit>
            => BotEntity<TEntityUnit>.Unit.Sample;
        public abstract record Response : ResponseMessage<TUnit>;
        public record Union : Unit<List<TUnit>>, IList<TUnit>
    {
        public Union(IEnumerable<TUnit> units = default)
        {
            if (units != null)
                Units.AddRange(units);
        }
        List<TUnit> union;
        protected List<TUnit> Units
            => union ?? (union = new List<TUnit>());
        public int Count => Units?.Count ?? 0;
        public bool IsReadOnly
            => false;

            TUnit IList<TUnit>.this[int index] { get => ((IList<TUnit>)Units)[index]; set => ((IList<TUnit>)Units)[index] = value; }
        public BotUnion this[int index] { get => ((IList<BotUnion>)Units)[index]; set => ((IList<BotUnion>)Units)[index] = value; }

        public static implicit operator List<TUnit>(Union unit)
            => unit.Units;
        public static implicit operator Union(List<TUnit> units)
            => new Union(units);
        public void Add(TUnit obj)
            => this.Units.Add(obj);
        public Union Add(params TUnit[] units)
        {
            Units.AddRange(units);
            return this;
        }

        public int IndexOf(TUnit item)
        {
            return ((IList<TUnit>)Units).IndexOf(item);
        }

        public void Insert(int index, TUnit item)
        {
            ((IList<TUnit>)Units).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Units?.RemoveAt(index);
        }

        public void Clear()
        {
            ((ICollection<TUnit>)Units).Clear();
        }

        public bool Contains(TUnit item)
        {
            return ((ICollection<TUnit>)Units).Contains(item);
        }

        public void CopyTo(TUnit[] array, int arrayIndex)
        {
            ((ICollection<TUnit>)Units).CopyTo(array, arrayIndex);
        }

        public bool Remove(TUnit item)
        {
            return ((ICollection<TUnit>)Units).Remove(item);
        }

        public IEnumerator<TUnit> GetEnumerator()
        {
            return Units?.GetEnumerator() ?? Enumerable.Empty<TUnit>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

            
        }
        public record Unit : Unit<TUnit>
        {
           
        }
    }
}

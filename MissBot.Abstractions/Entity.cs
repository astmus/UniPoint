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
    public record BotUnion<TUnit> : BotUnion where TUnit : Value
    {
        public void Add(TUnit obj)
            => base.Add(obj);
    }
    [JsonObject]
    public record BotUnion : Value, IList<Value>
    {
        public BotUnion(IEnumerable<Value> units = default)
        {
            if (units != null)
                Union.AddRange(units);
        }
        List<Value> union;
        protected List<Value> Union
            => union ?? (union = new List<Value>());
        public int Count => Union?.Count ?? 0;

        public bool IsReadOnly
            => false;

        Value IList<Value>.this[int index] { get => ((IList<Value>)Union)[index]; set => ((IList<Value>)Union)[index] = value; }
        public BotUnion this[int index] { get => ((IList<BotUnion>)Union)[index]; set => ((IList<BotUnion>)Union)[index] = value; }

        public static implicit operator List<Value>(BotUnion unit)
            => unit.Union;
        public static implicit operator BotUnion(List<Value> units)
            => new BotUnion(units);
        public void Add(Value obj)
            => this.Union.Add(obj);
        public BotUnion Add(params Value[] units)
        {
            Union.AddRange(units);
            return this;
        }
        public void Add(string obj)
            => this.Union.Add(new Value(obj));

        public int IndexOf(Value item)
        {
            return ((IList<Value>)Union).IndexOf(item);
        }

        public void Insert(int index, Value item)
        {
            ((IList<Value>)Union).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Union?.RemoveAt(index);
        }

        public void Clear()
        {
            ((ICollection<Value>)Union).Clear();
        }

        public bool Contains(Value item)
        {
            return ((ICollection<Value>)Union).Contains(item);
        }

        public void CopyTo(Value[] array, int arrayIndex)
        {
            ((ICollection<Value>)Union).CopyTo(array, arrayIndex);
        }

        public bool Remove(Value item)
        {
            return ((ICollection<Value>)Union).Remove(item);
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return Union?.GetEnumerator() ?? Enumerable.Empty<Value>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }

    public record MetaInfo(string key, MetaReference reference);
    public record MetaReference(WeakReference reference)
    {
        public object? Value
            => reference.Target;
        public T As<T>() where T : class
        {
            if (reference.Target is T value)
                return value;
            return default(T);
        }
    }
    
    public class MetaData : ConcurrentDictionary<string, object>
    {
        

        public T Get<T>([CallerMemberName] string name = default) 
        {
            if (TryGetValue(name, out var r) && r  is T value)
                return value;
            return default(T);
        }

        public TAny FirstOrNull<TAny>(string name)
        {
            return this.Where(x => x.Key == name && x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();
        }

        public T Set<T>(T value, [CallerMemberName] string name = default)
        {
            AddOrUpdate<T>(name,
                (k, w) => w,
                (k, o, w) => this[k] = w,
                value);
            return value;
        }
        public void Reset()
            => Clear();
    }

    public record Unit<TEntity> : Value
    {
        protected MetaData meta;
        public MetaData Meta
            => meta ?? (meta = new MetaData());
        public TEntity Value { get; set; }
        static Unit()
        {
            Instance = new Unit<TEntity>() with { Value = Sample = default(TEntity) };
        }
        public static Unit<TEntity> Init(TEntity entity)
            => Instance with { Value = entity };
        public static Unit<TValue> Init<TValue>(TValue entity = default) where TValue : Unit<TEntity>
            => Unit<TValue>.Init(entity);

        public static TEntity Sample { get; set; }
        public static Unit<TEntity> Instance { get; protected set; }
        protected T Set<T>(T value, [CallerMemberName] string name = default) 
        {
            return Meta.Set(value, name);
        }
        protected T Get<T>([CallerMemberName] string name = default) 
        {
            return Meta.Get<T>(name);
        }
        //public static Unit<TEntity> operator +(Unit<TEntity> a, Unit<TEntity> b)
        //   => new Unit<KeyValuePair<Value, Value>>(new KeyValuePair<Value, Value>(a.unit, b.unit));

    }
    public record Value(object value = default)
    {
        public static implicit operator Value(int unit)
            => new Value(unit);
        public static implicit operator int(Value unit)
            => Convert.ToInt32(unit.value);
        public static Value operator -(Value a, Value b)
            => a + (-b);
    }

    public static class BotEntity<TUnit>
    {
        public abstract record Response : ResponseMessage<TUnit>;
        public record Unit : BotUnion;
    }
}

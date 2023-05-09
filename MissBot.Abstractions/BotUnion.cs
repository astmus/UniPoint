using System.Collections;


namespace MissBot.Abstractions
{
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
}

using System.Collections;
using MissCore.Collections;

namespace MissCore.Bot
{
    public record BotUnion : Unit, IList<Unit>
    {
        public BotUnion(IEnumerable<Unit> units = default)
        {
            if (units != null)
                Union.AddRange(units);
        }
        List<Unit> union;
        protected List<Unit> Union
            => union ?? (union = new List<Unit>());
        public int Count => Union?.Count ?? 0;
        public bool IsReadOnly
            => false;

        Unit IList<Unit>.this[int index] { get => ((IList<Unit>)Union)[index]; set => ((IList<Unit>)Union)[index] = value; }
        public BotUnion this[int index] { get => ((IList<BotUnion>)Union)[index]; set => ((IList<BotUnion>)Union)[index] = value; }

        public static implicit operator List<Unit>(BotUnion unit)
            => unit.Union;
        public static implicit operator BotUnion(List<Unit> units)
            => new BotUnion(units);
        public void Add(Unit obj)
            => Union.Add(obj);
        public BotUnion Add(params Unit[] units)
        {
            Union.AddRange(units);
            return this;
        }


        public int IndexOf(Unit item)
        {
            return ((IList<Unit>)Union).IndexOf(item);
        }

        public void Insert(int index, Unit item)
        {
            ((IList<Unit>)Union).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Union?.RemoveAt(index);
        }

        public void Clear()
        {
            ((ICollection<Unit>)Union).Clear();
        }

        public bool Contains(Unit item)
        {
            return ((ICollection<Unit>)Union).Contains(item);
        }

        public void CopyTo(Unit[] array, int arrayIndex)
        {
            ((ICollection<Unit>)Union).CopyTo(array, arrayIndex);
        }

        public bool Remove(Unit item)
        {
            return ((ICollection<Unit>)Union).Remove(item);
        }

        public IEnumerator<Unit> GetEnumerator()
        {
            return Union?.GetEnumerator() ?? Enumerable.Empty<Unit>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }
}

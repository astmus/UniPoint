using System.Collections;

namespace MissBot.Abstractions
{
    public static partial class BotEntity<TUnit>
    {
        public static TEntityUnit Instance<TEntityUnit>() where TEntityUnit : Unit<TUnit>
            => Unit<TEntityUnit>.Sample;
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
    }
}

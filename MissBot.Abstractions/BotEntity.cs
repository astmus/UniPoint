using System.Collections;

namespace MissBot.Abstractions
{
    public static class BotEntity<TUnit>
    {
        public record Collection : Union<TUnit>;
        public record Content : ContentUnit<TUnit>;
        public static TEntityUnit Instance<TEntityUnit>() where TEntityUnit : Unit<TUnit>
            => Unit<TEntityUnit>.Sample;
        public abstract record Response : ResponseMessage<TUnit>;
        #region MyRegion
        [JsonArray]
        public record Union : Unit, IList<TUnit>
        {
            public Union()
            {
                
            }
            public Union(IEnumerable<TUnit> units = default)
            {
                if (units != null)
                    Content.AddRange(units);
            }
            List<TUnit> union;
            protected List<TUnit> Content
                => union ?? (union = new List<TUnit>());

            public int Count => ((ICollection<TUnit>)Content).Count;

            public bool IsReadOnly => ((ICollection<TUnit>)Content).IsReadOnly;

            public TUnit this[int index] { get => ((IList<TUnit>)Content)[index]; set => ((IList<TUnit>)Content)[index] = value; }

            public int IndexOf(TUnit item)
            {
                return ((IList<TUnit>)Content).IndexOf(item);
            }

            public void Insert(int index, TUnit item)
            {
                ((IList<TUnit>)Content).Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                Content?.RemoveAt(index);
            }

            public void Clear()
            {
                ((ICollection<TUnit>)Content).Clear();
            }

            public bool Contains(TUnit item)
            {
                return ((ICollection<TUnit>)Content).Contains(item);
            }

            public void CopyTo(TUnit[] array, int arrayIndex)
            {
                ((ICollection<TUnit>)Content).CopyTo(array, arrayIndex);
            }

            public bool Remove(TUnit item)
            {
                return ((ICollection<TUnit>)Content).Remove(item);
            }

            public IEnumerator<TUnit> GetEnumerator()
            {
                return Content?.GetEnumerator() ?? Enumerable.Empty<TUnit>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            //public static implicit operator List<TUnit>(Union unit)
            //    => unit.Units;
            //public static implicit operator Union(List<TUnit> units)
            //    => new Union(units);

            public Union Add(params TUnit[] units)
            {
                Content.AddRange(units);
                return this;
            }
                    
            public void Add(TUnit item)
            {
                ((ICollection<TUnit>)Content).Add(item);
            }

     
        } 
        #endregion
    }
}

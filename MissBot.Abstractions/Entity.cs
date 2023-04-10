using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions
{
        
        public record Unit<TValue> : BaseEntity;
        public record Union<TValue> : Unit<TValue>;
    public abstract record BaseEntity
    {
    }
    public abstract record BotEntity<TUnit>
    {
        public record Unit : Unit<TUnit>;
        public record Union : Unit, IEnumerable<Unit>
        {
            IList<Unit> Units { get; } = new List<Unit>();

            public IEnumerator<Unit> GetEnumerator()
                => Units.GetEnumerator();

            public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit
            {
                Units.Add(unit);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)Units).GetEnumerator();
            }
        }
    }
}

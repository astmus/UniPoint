using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions
{

    public record Unit<TValue> : BaseEntity.BotUnit;
    public record Union<TValue> : Unit<TValue>;
    public abstract record BaseEntity
    {
        public record BotUnit : BaseEntity;
        public record BotUnion : BotUnit, IEnumerable<BotUnit>
        {
            IList<BotUnit> Units { get; } = new List<BotUnit>();

            public IEnumerator<BotUnit> GetEnumerator()
                => Units.GetEnumerator();

            public virtual void Write<TUnitData>(TUnitData unit) where TUnitData : BotUnit
            {
                Units.Add(unit);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)Units).GetEnumerator();
            }
        }
    }
    public abstract record BotEntity<TUnit> : BaseEntity.BotUnion
    {
        public abstract record Response : ResponseMessage<TUnit>;
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

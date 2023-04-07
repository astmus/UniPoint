
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissBot.Abstractions
{
    public record BotEntity<TUnit> : BaseEntity.Unit<TUnit>
    {
        [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public  record Unit : Unit<TUnit>
        {
           
        }

        [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public abstract record Union : Unit, IEnumerable<Unit>
        {
            IList<Unit> Units { get; } = new List<Unit>();          

            public IEnumerator<Unit> GetEnumerator()
                => Units.GetEnumerator();            

            public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit
            {
                Units.Add(unit);
            }

            IEnumerator IEnumerable.GetEnumerator()
                => ((IEnumerable)Units).GetEnumerator();
            
        }

    }

}

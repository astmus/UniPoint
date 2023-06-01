using MissBot.Common.Extensions;

namespace MissBot.Abstractions.Entities
{
    public interface IBotEntity
    {
        string Unit { get; set; }
        string Entity { get; }
    }
    public interface IBotDataEntity
    {
        string Payload { get; set; }
    }
    public interface IBotUnit : IBotEntity, IBotDataEntity
    {
        string Description { get; set; }
        string Template { get; set; }
        string Parameters { get; }
        string this[int index]
            => GetParameters().ElementAtOrDefault(index);

        IEnumerable<string> GetParameters()
        {
            IList<string> parameters = new List<string>();

            var enumerator = Parameters.SplitParameters().GetEnumerator();
            while (enumerator.MoveNext())            
                parameters.Add(enumerator.Current);

            return parameters;
        }

    }

    public interface IBotUnit<TUnit> : IBotEntity
    {
        void SetUnitActions<TSub>(TSub unit) where TSub : BaseUnit;
        public IEnumerable<IBotUnit> Units { get; }
    }
}

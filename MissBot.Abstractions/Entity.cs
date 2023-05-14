using System.Text.Json.Nodes;

namespace MissBot.Abstractions
{
    public delegate IEnumerable<string> FieldNamesSelector<TUnit>(TUnit entity);
    public delegate (string field, string value) WhereSelector<TUnit>(TUnit entity);
    public delegate void LoadSelector<TUnit>(TUnit entity);
    [JsonArray]
    public record Union<TUnit> : Unit<TUnit>.Union, IList<TUnit>
    {
        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Content.Add(obj);
    }

    public interface IUnitCollection
    {
        string ToString();
    }
}

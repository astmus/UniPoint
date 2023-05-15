using System.Text.Json.Nodes;

namespace MissCore.Collections
{       
    [JsonArray]
    public class Union<TUnit> : Unit<TUnit>.Collection, IList<TUnit>, IUnitCollection
    {
        public void Add<TEntity>(TEntity obj) where TEntity : TUnit
                => Add(obj);
    }

    public interface IUnitCollection
    {
        string ToString();
    }
}

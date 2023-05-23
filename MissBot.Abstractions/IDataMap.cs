using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public interface IDataMap
    {
        void JoinData<TEntity>(TEntity entity);
        TObject ReadObject<TObject>(string name);
    }
}

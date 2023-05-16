using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public interface IDataMap
    {
        void JoinData<TEntity>(TEntity entity);
        TD Read<TD>(string name);
    }
}

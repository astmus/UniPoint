using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public interface IDataMap
    {
        JToken Parse<TData>(TData value);
        TD Read<TD>(string name);
    }
}

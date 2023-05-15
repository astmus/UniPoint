namespace MissBot.Abstractions
{
    public interface IDataMap
    {
        void Parse<TData>(TData value);
        TD Read<TD>(string name);
    }
}

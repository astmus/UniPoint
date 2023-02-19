namespace MissCore.Abstractions
{
    public interface IResponseChannel
    {
        void Write<T>(T data);
        Task WriteAsync<T>(T data, CancellationToken cancel);
    }
}

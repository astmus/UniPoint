namespace MissBot.Abstractions
{
    public interface IResponseChannel
    {
        Task SendHandlingStart();
        void SetContext(IHandleContext context);
        Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class;  
    }
}

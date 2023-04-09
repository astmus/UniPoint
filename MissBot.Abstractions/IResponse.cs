namespace MissBot.Abstractions
{
    public interface IResponse
    {
        Task SendHandlingStart();
        void SetContext(IHandleContext context);
        Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class;  
    }
}

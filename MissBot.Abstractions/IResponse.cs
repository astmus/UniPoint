namespace MissBot.Abstractions
{
    public interface IResponse
    {
        Task SendHandlingStart();
        void SetContext(IHandleContext context);
        Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        Task<IResponseChannel> CreateAsync<T>(T data, CancellationToken cancel) where T : class;
        IResponse<T> Create<T>(T data) where T : class;
    }
}

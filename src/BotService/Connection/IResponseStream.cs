namespace BotService.Connection
{
    public interface IResponseStream
    {
        void Write<T>(T value);
        Task WriteAsync<T>(T value);
        Task OpenAsync();
        Task CloseAsync();
        Task FlushAsync(CancellationToken cancel);
    }
}

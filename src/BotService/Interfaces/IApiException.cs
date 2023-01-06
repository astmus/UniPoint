namespace BotService.Interfaces
{
    public interface IApiException
    {
        string Description { get; }
        int ErrorCode { get; }
        ResponseErrorInfo? Parameters { get; }
    }
}

namespace MissBot.Entities
{
    public interface IBotRequest
    {
        HttpMethod Method { get; }
        string MethodName { get; }
        bool IsWebhookResponse { get; set; }
        HttpContent? ToHttpContent();
    }
    public interface IBotRequest<TResponse> : IBotRequest { }
}

using System.Net.Http;
using MissBot.Entities;
using Telegram.Bot.Requests.Abstractions;

namespace MissBot.Abstractions.Args;

/// <summary>
/// Provides data for MakingApiRequest event
/// </summary>
public class ApiRequestEventArgs : EventArgs
{
    /// <summary>
    /// Bot API Request
    /// </summary>
    public IBotRequest Request { get; }

    /// <summary>
    /// HTTP Request Message
    /// </summary>
    public HttpRequestMessage HttpRequestMessage { get; }

    /// <summary>
    /// Initialize an <see cref="ApiRequestEventArgs"/> object
    /// </summary>
    /// <param name="request"></param>
    /// <param name="httpRequestMessage"></param>
    public ApiRequestEventArgs(IBotRequest request, HttpRequestMessage httpRequestMessage = default)
    {
        Request = request;
        HttpRequestMessage = httpRequestMessage;
    }
}

using MediatR;

namespace BotService.Interfaces
{
    public interface IBotClient
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task WriteAsync(object data, CancellationToken cancel = default);
        Task<TResponse> SendClassicRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        //IBotClient SetupContext(IHandleContext context);
        //IBotChannel Channel { get; }
    }
}


using MissCore.Configuration;

namespace MissBot.Abstractions
{
    public delegate Task AsyncHandler(IHandleContext context);
    public delegate IBotClient BotClientDelegate();
    public delegate Task ExecuteHandler(CancellationToken cancel = default);
    public delegate Task SetupHandler<T>(T data, IContext<T> context);
    public delegate Task AsyncHandler<T>(T data, IContext<T> context);
    public delegate Task AsyncGenericHandler<T>(IContext<T> context);
}

using MissBot.Abstractions.Configuration;

namespace MissBot.Abstractions
{
    public delegate Task AsyncHandler(IHandleContext context);
    public delegate object AsyncInputHandler(IHandleContext context, string input, string parameterName);
    public delegate Task ExecuteHandler(CancellationToken cancel = default);
    public delegate Task AsyncHandler<in T>(T data, CancellationToken cancel = default);
    public delegate Task AsyncGenericHandler<in T>(T data, IHandleContext context);
}

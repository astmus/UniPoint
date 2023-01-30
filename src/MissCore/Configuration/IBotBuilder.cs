using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissCore.Configuration
{

    public interface IBotBuilder
    {
        IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware);

        IBotBuilder Use(Func<IHandleContext, HandleDelegate> component);

        IBotBuilder Use<THandler>() where THandler : IAsyncHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler : IAsyncHandler;

        IBotBuilder Use<TCommand, THandler>() where THandler : BaseHandler<TCommand> where TCommand : BotCommand;
        HandleDelegate Build();
        IBot BuildClient(IServiceScope scope);
    }
    public interface IBotBuilder<out TBot> : IBotBuilder where TBot : class, IBot
    {
        IBotBuilder<TBot> UseCommndFromAttributes();
    }
}

using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;
using MissCore.Entities;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissCore.Configuration
{

    public interface IBotBuilder
    {
        IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware);

        IBotBuilder Use(Func<IHandleContext, HandleDelegate> component);

        IBotBuilder Use<THandler>() where THandler : class, IAsyncHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler :class, IAsyncHandler;

        IBotBuilder Use<TCommand, THandler>() where THandler : BaseHandler<TCommand> where TCommand : BotCommand;
        HandleDelegate Build();
    }
    public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
    {
        IServiceProvider BotServices();
        IBotBuilder<TBot> UseCommndFromAttributes();
        IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler :class, IAsyncHandler<Update<TBot>>;
    }
}

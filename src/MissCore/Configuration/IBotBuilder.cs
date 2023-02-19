using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Entities;

namespace MissCore.Configuration
{

    public interface IBotBuilder
    {
        IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware);

        IBotBuilder Use(Func<IHandleContext, HandleDelegate> component);

        IBotBuilder Use<THandler>() where THandler : class, IAsyncHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler :class, IAsyncHandler;

        IBotBuilder Use<TCommand, THandler>() where THandler :class, IAsyncHandler<TCommand>, IAsyncBotCommansHandler where TCommand :class, IBotCommandData;
        HandleDelegate BuildHandler();
        void Build();
    }
    public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
    {
        IServiceCollection BotServices { get; }
        IBotServicesProvider BotServicesProvider();
        IBotBuilder<TBot> UseCommndFromAttributes();
        IBotBuilder<TBot> UseContextHandler<THandler>() where THandler: class, IContextHandler<Update<TBot>>;
        IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler :class, IAsyncHandler<Update<TBot>>;
    }
}

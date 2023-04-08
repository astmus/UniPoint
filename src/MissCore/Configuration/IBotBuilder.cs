using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Entities;

namespace MissCore.Configuration
{

    public interface IBotBuilder
    {
        IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware);

        IBotBuilder Use(Func<IHandleContext, AsyncHandler> component);

        IBotBuilder AddHandler<THandler>() where THandler : class, IAsyncHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler :class, IAsyncHandler;

        IBotBuilder Use<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : class, IBotCommand;
        AsyncHandler BuildHandler();
        void Build();
    }
    public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
    {
        IServiceCollection BotServices { get; }
        IBotServicesProvider BotServicesProvider();
        IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler;
        IBotBuilder<TBot> UseCommndFromAttributes();
        IBotBuilder<TBot> UseContextHandler<THandler>() where THandler: class, IContextHandler<Update<TBot>>;
        IBotBuilder<TBot> UseCommandHandler<THandler>() where THandler : class, IAsyncBotCommandHandler;
        IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler :class, IAsyncUpdateHandler<Update<TBot>>;
    }
}

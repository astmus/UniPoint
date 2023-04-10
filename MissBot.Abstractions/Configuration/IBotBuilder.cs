using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using Telegram.Bot.Types;

namespace MissCore.Configuration
{

    public interface IBotBuilder
    {
        IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware);

        IBotBuilder Use(Func<IHandleContext, AsyncHandler> component);

        IBotBuilder AddHandler<THandler>() where THandler : class, IAsyncHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler :class, IAsyncHandler;
        

        IBotBuilder Use<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : class, IBotCommand;
        IBotBuilder Use<TCommand, THandler, TResponse>() where
            THandler : BotCommandHandler<TCommand> where
            TCommand : class, IBotCommand where
            TResponse :class, IResponse<TCommand>;

        AsyncHandler BuildHandler();
        void Build();
    }
    public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
    {
        IServiceCollection BotServices { get; }
        IBotServicesProvider BotServicesProvider();
        IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler;
        IBotBuilder<TBot> UseCommndFromAttributes();
        IBotBuilder<TBot> UseContextHandler<THandler>() where THandler: class, IContextHandler<TBot>;
        IBotBuilder<TBot> UseInlineHandler<THandler>() where THandler : class, IAsyncHandler<InlineQuery>;
        IBotBuilder<TBot> UseCommandHandler<THandler>() where THandler : class, IAsyncBotCommandHandler;
        IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler :class, IAsyncUpdateHandler<TBot>;
    }
}

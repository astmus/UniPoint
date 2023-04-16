using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using Telegram.Bot.Types;

namespace MissBot.Abstractions.Configuration
{

    public interface IBotBuilder
    {
        IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware);

        IBotBuilder Use(Func<IHandleContext, AsyncHandler> component);

        IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction : class, IEntityAction;

        IBotBuilder Use<THandler>(THandler handler) where THandler : class, IAsyncHandler;

        IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BotCommand, IBotCommand;
        IBotBuilder AddCommand<TCommand, THandler, TResponse>() where
            THandler : BotCommandHandler<TCommand> where
            TCommand : BotCommand, IBotCommand where
            TResponse : class, IResponse<TCommand>;

        AsyncHandler BuildHandler();
        void Build();
    }
    public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
    {
        IServiceCollection BotServices { get; }
        IBotServicesProvider BotServicesProvider();
        IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler;
        IBotBuilder<TBot> UseCommndFromAttributes();
        IBotBuilder<TBot> UseInlineHandler<THandler>() where THandler : class, IAsyncHandler<InlineQuery>;
        IBotBuilder<TBot> UseCallbackDispatcher<THandler>() where THandler : class, IAsyncHandler<CallbackQuery>;
        IBotBuilder<TBot> UseInlineAnswerHandler<THandler>() where THandler : class, IAsyncHandler<ChosenInlineResult>;
        IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandDispatcher;
        IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>;
        IBotBuilder<TBot> UseCommandsRepository<THandler>() where THandler : class, IRepository<BotCommand>;
    }
}

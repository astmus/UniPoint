using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results;
using BotCommand = MissBot.Abstractions.Entities.BotCommand;

namespace MissBot.Abstractions.Configuration
{
    public interface IBotBuilder
    {
        IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware);
        IBotBuilder Use(Func<IHandleContext, AsyncHandler> component);
        IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction : class, IBotUnitCommand;
        IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BotCommand, IBotCommand;
        IBotBuilder AddHandler<THandler>() where THandler : class, IAsyncHandler<THandler>;
        AsyncHandler BuildHandler();
        void Build();
    }

    public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
    {
        IBotBuilder<TBot> AddRepository<TRepository, TImplementatipon>() where TRepository : class, IRepository where TImplementatipon : class, TRepository;        
        IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler;        
        IBotBuilder<TBot> UseInlineHandler<THandler>() where THandler : class, IAsyncHandler<InlineQuery>;
        IBotBuilder<TBot> UseCallbackDispatcher<THandler>() where THandler : class, IAsyncHandler<CallbackQuery>;
        IBotBuilder<TBot> UseInlineAnswerHandler<THandler>() where THandler : class, IAsyncHandler<ChosenInlineResult>;
        IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandDispatcher;
        IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>;
        IBotBuilder<TBot> UseCommandsRepository<THandler>() where THandler : class, IBotCommandsRepository;
        IBotBuilder<TBot> UseMessageHandler<THandler>() where THandler : class, IAsyncHandler<Message>;
    }
}

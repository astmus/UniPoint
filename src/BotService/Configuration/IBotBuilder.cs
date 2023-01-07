using System;
using MissBot.Handlers;
using MissBot.Interfaces;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace BotService.Configuration
{
    public interface IBotCommands
    {
        IBotBuilder AddCommndFromAttributes<TBot>() where TBot : class, IBot;
    }

    public interface IBotBuilder : IBotCommands
    {
        IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware);

        IBotBuilder Use(Func<IHandleContext, HandleDelegate> component);

        IBotBuilder Use<THandler>() where THandler : IAsyncHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler : IAsyncHandler;

        IBotBuilder Use<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand:BotCommand;

        IBotHost BuildHostService();

        HandleDelegate Build();
    }
}

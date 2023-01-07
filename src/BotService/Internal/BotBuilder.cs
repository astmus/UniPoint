using BotService.Configuration;
using MissBot.Handlers;
using MissBot.Interfaces;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace BotService.Internal
{
    internal class BotBuilder : IBotBuilder
    {
        internal HandleDelegate HandlerDelegate { get; private set; }
        static internal IBotBuilder Default
            => new BotBuilder();
        private readonly ICollection<Func<HandleDelegate, HandleDelegate>> _components;
        private List<IBotCommandInfo> _botCommands;

        public BotBuilder()
        {
            _components = new List<Func<HandleDelegate, HandleDelegate>>();
        }

        public IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware)
        {
            throw new NotImplementedException();
        }



        public IBotBuilder Use<THandler>() where THandler : IAsyncHandler
        {
            _components.Add(
                next =>
                context =>
                     context.NextHandler<THandler>().ExecuteAsync(context, next)
            );

            return this;
        }

        public IBotBuilder Use<THandler>(THandler handler) where THandler : IAsyncHandler
        {
            _components.Add(next =>
                context
                    => handler.ExecuteAsync(context, next)
            );

            return this;
        }

        public IBotBuilder Use(Func<IHandleContext, HandleDelegate> component)
        {
            throw new NotImplementedException();
        }


        public HandleDelegate Build()
        {
            HandleDelegate handle = context =>
            {
                context.Info.IsHandled = true;
#if DEBUG
                Console.WriteLine("No handler for update {0} of type {1}.", context.Info.UpdateId, context.Info.ChatId);
#endif
                return Task.FromResult(1);
            };

            foreach (var component in _components.Reverse())
                handle = component(handle);


            return HandlerDelegate = handle;
        }

        #region Commands       


       


        public IBotClient BuildService()
        {
            throw new NotImplementedException();
        }

        public IBotBuilder Use<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }

        public IBotHost BuildHostService()
        {
            throw new NotImplementedException();
        }

        public IBotBuilder AddCommndFromAttributes<TBot>() where TBot:class, IBot
        {            
            _botCommands.AddRange(typeof(TBot).GetAttributedCommands<TBot>());
            return this;
        }


        #endregion
    }
}

using BotService.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Handlers;
using MissBot.Interfaces;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Handlers;
using Telegram.Bot.Types;
using static System.Formats.Asn1.AsnWriter;

namespace BotService.Internal
{
    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot
    {
        internal static BotBuilder<TBot> instance;
        internal static BotBuilder<TBot> Instance { get=> instance ?? (instance = new BotBuilder<TBot>());  }

        public IBotBuilder<TBot> UseCommndFromAttributes()
        {
            _botCommands.AddRange(typeof(TBot).GetAttributedCommands<TBot>());
            return this;
        }
        public override IBot BuildClient(IServiceScope scope)
        {            
            Build();
            var bot = scope.ServiceProvider.GetRequiredService<TBot>();
            bot.SetScope(scope);
            bot.Handler = ()=> Build();
            return bot;
        }     

        
    }

    internal  class BotBuilder : IBotBuilder
    {
        protected internal Func<IServiceScope, IBot> func;
        internal HandleDelegate HandlerDelegate { get; private set; }
        
        protected readonly ICollection<Func<HandleDelegate, HandleDelegate>> _components;
        protected List<IBotCommandInfo> _botCommands = new List<IBotCommandInfo>();

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

       

        public IBotBuilder Use<TCommand, THandler>() where THandler : BaseHandler<TCommand> where TCommand : BotCommand
        {
            _components.Add(
                next =>
                context =>
                     context.NextHandler<THandler>().ExecuteAsync(context, next));
            return this;
        }

        

        public virtual IBot BuildClient(IServiceScope scope)
            => scope.ServiceProvider.GetRequiredService<IBot>();



        #endregion
    }
}

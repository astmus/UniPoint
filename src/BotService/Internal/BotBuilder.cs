using MissBot.Interfaces;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotService.Internal
{
    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot
    {
        internal static BotBuilder<TBot> instance;
        internal static BotBuilder<TBot> Instance { get=> instance;  }
        internal override IServiceCollection Services
        { get; set; }
        internal static BotBuilder<TBot> GetInstance()
        {            
            return Instance;
        }
        static BotBuilder()
        {
            instance = new BotBuilder<TBot>();
            instance.Services = new ServiceCollection(); ;
        }
        internal BotBuilder() {
            Services = Instance?.Services;
        }
        public IBotBuilder<TBot> UseCommndFromAttributes()
        {
            _botCommands.AddRange(typeof(TBot).GetAttributedCommands<TBot>());
            return this;
        }
        IServiceProvider sp;
        public IServiceProvider BotServices()
            => sp ??= Instance.Services.BuildServiceProvider();

        public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler :class, IAsyncHandler<Update<TBot>>
        {
            Services.AddScoped<IAsyncHandler<Update<TBot>>, THandler>();
            return this;
        }
    }

    internal abstract class BotBuilder : IBotBuilder
    {
        internal HandleDelegate HandlerDelegate { get; private set; }
        internal abstract IServiceCollection Services { get; set; }
        protected readonly ICollection<Func<HandleDelegate, HandleDelegate>> _components;
        protected List<IBotCommandInfo> _botCommands = new List<IBotCommandInfo>();

        internal BotBuilder()
        {
            _components = new List<Func<HandleDelegate, HandleDelegate>>();
        }

        public IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware)
        {
            throw new NotImplementedException();
        }

        public IBotBuilder Use<THandler>() where THandler :class, IAsyncHandler
        {
            Services.AddScoped<THandler>();
            _components.Add(
                next =>
                context =>
                     context.NextHandler<THandler>().ExecuteAsync(context, next)
            );

            return this;
        }

        public IBotBuilder Use<THandler>(THandler handler) where THandler :class, IAsyncHandler
        {
            Services.AddScoped<THandler>();
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
            Services.BuildServiceProvider(true);
            HandleDelegate handle = context =>
            {
                context.Update.IsHandled = true;
#if DEBUG
                Console.WriteLine("No handler for update {0} of type {1}.", context.Update.UpdateId, context.Update.ChatId);
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
            Services.AddScoped<THandler>();
            _components.Add(
                next =>
                context =>
                     context.NextHandler<THandler>().ExecuteAsync(context, next));
            return this;
        }

        #endregion
    }
}

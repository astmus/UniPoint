using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;
using MissBot.Abstractions.Presentation;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Query;
using MissBot.Entities.Results;

using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Context;
using MissCore.Handlers;
using MissCore.Presentation.Convert;
using MissCore.Response;
using MissBot.Identity;
using System.Diagnostics;
using LinqToDB;
using MissCore.Storage;

namespace BotService.Internal
{
	internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot
	{
		Func<Action<IServiceCollection>, IBotBuilder<TBot>> WithBot;
		internal static BotBuilder<TBot> GetInstance(IHostBuilder rootHost)
		{
			rootHost.ConfigureServices((h, s)
				=> s.AddScoped<IBotServicesProvider, BotServicesProvider>());
			return new BotBuilder<TBot>(rootHost);
		}
		internal BotBuilder(IHostBuilder rootHost)
		{
			host = rootHost;
			WithBot = action =>
			{
				host.ConfigureServices(services => action(services));
				return this;
			};
		}

		public IBotBuilder<TBot> UseCommandsRepository<THandler>() where THandler : class, IBotCommandsRepository
			=> WithBot(services => services.AddScoped<IBotCommandsRepository, THandler>());

		public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>
			=> WithBot(services => services.AddScoped<IAsyncUpdateHandler<TBot>, THandler>());

		public IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandDispatcher
		{
			_components.Add(
				next =>
				context =>
					{
						if (context.Get<Update<TBot>>() is UnitUpdate upd && upd.IsCommand)
							return context.GetAsyncHandler<IAsyncBotCommandDispatcher>().AsyncDelegate(context.SetNextHandler(next));
						else
							return next(context);
					});

			return WithBot(services =>
			{
				services.AddScoped<IAsyncBotCommandDispatcher, THandler>()
							.AddScoped<IResponse<BotCommand>, BotCommandResponse<BotCommand>>();
			});
		}
		public IBotBuilder<TBot> UseInlineAnswerHandler<THandler>() where THandler : class, IAsyncHandler<ChosenInlineResult>
		{
			WithBot(services =>
			{
				services.AddScoped<IAsyncHandler<ChosenInlineResult>, THandler>();
				//Services.AddScoped<IResponse<ChosenInlineResult>, Response<ChosenInlineResult>>();
				services.AddScoped<IContext<ChosenInlineResult>, Context<ChosenInlineResult>>();
			});

			_components.Add(
				next =>
				context =>
				{
					if (context.Contains(Id<ChosenInlineResult>.Instance))
						return context.GetAsyncHandlerOf<ChosenInlineResult>().AsyncDelegate(context.SetNextHandler(next));
					else
						return next(context);
				});
			return this;
		}
		public IBotBuilder<TBot> UseCallbackDispatcher<THandler>() where THandler : class, IAsyncHandler<CallbackQuery>
		{
			host.ConfigureServices((h, Services) =>
			{
				Services.AddScoped<IAsyncHandler<CallbackQuery>, THandler>();
				Services.AddScoped<IResponseNotification, CallBackNotification>();
				//Services.AddScoped<IResponse<CallbackQuery>, Response<CallbackQuery>>();
				Services.AddScoped<IContext<CallbackQuery>, Context<CallbackQuery>>();
			});
			_components.Add(
				next =>
				context =>
				{
					if (context.Contains(Id<CallbackQuery>.Instance))
						return context.GetAsyncHandlerOf<CallbackQuery>().AsyncDelegate(context.SetNextHandler(next));
					else
						return next(context);
				});
			return this;
		}

		public IBotBuilder<TBot> UseInlineHandler<TUnit, THandler>() where THandler : class, IAsyncHandler<InlineQuery<TUnit>> where TUnit : BaseUnit, IUnit
		{
			_components.Add(
				next =>
				context =>
				{
					if (context.Contains(Id<InlineQuery>.Instance))
						return context.GetAsyncHandlerOf<InlineQuery<TUnit>>().AsyncDelegate(context.SetNextHandler(next));
					else
						return next(context);
				});
			return WithBot(services =>
				{
					services.AddScoped<IAsyncHandler<InlineQuery<TUnit>>, THandler>();
					services.AddScoped<InlineResponse<TUnit>>();
					services.AddScoped<IContext<InlineQuery<TUnit>>, Context<InlineQuery<TUnit>>>();
				});
		}

		public IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandleComponent
		{
			_components.Add(
				next =>
				context =>
					 context.GetBotService<THandler>().HandleAsync(context, next));

			return WithBot(services => services.AddScoped<THandler>());
		}

		public IBotBuilder<TBot> AddGenericRepository<TRepository, TImplementatipon>() where TRepository : class, IRepository where TImplementatipon : class, TRepository, IRepository
			=> WithBot(services => services.AddScoped<TRepository, TImplementatipon>());


		public IBotBuilder<TBot> UseMessageHandler<THandler>() where THandler : class, IAsyncHandler<Message>
		{
			host.ConfigureServices((h, Services)
				=>
				{
					Services.AddScoped<IAsyncHandler<Message>, THandler>();
					Services.AddScoped<IContext<Message>, Context<Message>>();
					//Services.AddScoped<IResponse<Message>, Response<Message>>();
				});

			_components.Add(
				next =>
				context =>
				{
					if (context.Contains(Id<Message>.Instance))
						return context.GetAsyncHandlerOf<Message>().AsyncDelegate(context.SetNextHandler(next));
					else
						return next(context);
				});
			return this;
		}

		public IBotBuilder<TBot> AddUnit<TUnit, TUnitActionHandler>() where TUnit : BaseUnit where TUnitActionHandler : class, IAsyncUnitActionHanlder<TUnit>
		{
			return WithBot(services =>
			{
				Debug.WriteLine(Id<TUnit>.UnderlineType.FullName);
				services.AddScoped<IAsyncUnitActionHanlder<TUnit>, TUnitActionHandler>();
			});
		}

		public IBotBuilder<TBot> AddRepository<TUnit, TImplementatipon>() where TUnit : class where TImplementatipon : class, IRepository<TUnit>
			=> WithBot(services
				=>
			{
				services.AddScoped<IRepository<TUnit>, TImplementatipon>();
				//services.AddOptions<DataOptions<UserDataContext>>().Configure(opt => opt.
			});
	}

	internal abstract class BotBuilder : IBotBuilder
	{
		protected IHostBuilder host;
		delegate IHostBuilder BuildHostDelegate(Action<HostBuilderContext, IServiceCollection> services);
		delegate IHostBuilder BuildUnitsDelegate(Action<HostBuilderContext, IServiceCollection> services);
		delegate IHostBuilder BuildBotDelegate(IServiceCollection services);
		internal AsyncHandler HandlerDelegate { get; private set; }
		protected readonly ICollection<Func<AsyncHandler, AsyncHandler>> _components;
		Func<Action<BuildHostDelegate>, IBotBuilder> WithHost;
		Func<Action<IServiceCollection>, IBotBuilder> WithBot;
		Func<Action<IServiceCollection>, IBotUnitBuilder> WithUnit;

		internal BotBuilder()
		{
			_components = new List<Func<AsyncHandler, AsyncHandler>>();

			WithHost = action =>
			{
				action(host.ConfigureServices);
				return this;
			};

			WithBot = action =>
			{
				host.ConfigureServices(services => action(services));
				return this;
			};

			WithUnit = action =>
			{
				host.ConfigureServices(services => action(services));
				return this;
			};
		}

		IBotBuilder IBotBuilder.AddInputParametersHandler()
			=> WithBot(services
				=> services.AddScoped<InputParametersHandler>());

		public IBotBuilder Use(Func<IHandleContext, AsyncHandler> component)
		{
			throw new NotImplementedException();
		}


		public AsyncHandler BuildHandler()
		{
			AsyncHandler handle = context =>
			{
				Update upd = context.Any<Update>();
				context.IsHandled = true;
#if DEBUG
				Console.WriteLine("\t\tNo handler for update {0} of type {1}.", upd.Id, upd.Type);
#endif
				return Task.FromResult(1);
			};

			foreach (var component in _components.Reverse())
				handle = component(handle);

			return HandlerDelegate = handle;
		}
		#region Units
		public IBotUnitBuilder AddResponseUnit<TUnit>() where TUnit : BaseBotUnit
		   => WithUnit(services => services.AddScoped<UnitSerializeConverter<TUnit>>());

		#endregion

		#region Commands    

		public IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BaseAction
		{
			host.ConfigureServices((h, Services)
				=>
			{
				Services.AddScoped<IAsyncHandler<TCommand>, THandler>();
				Services.AddScoped<IResponse<TCommand>, BotCommandResponse<TCommand>>();
				Services.AddScoped<IContext<TCommand>, Context<TCommand>>();
			});

			//_components.Add(
			//    next =>
			//    context =>
			//         context.BotServices.
			//            GetRequiredService<IAsyncBotCommandHandler>().HandleAsync<TCommand>(context));
			return this;
		}

		public void Build()
		{
			if (HandlerDelegate is null)
				BuildHandler();
		}

		public IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction : class, IBotAction
			=> WithBot(services =>
			{
				services.AddScoped<IAsyncHandler<TAction>, THandler>();
				//Services.AddScoped<IResponse<TAction>, Response<TAction>>();
				services.AddScoped<IContext<TAction>, Context<TAction>>();
			});

		IBotBuilder IBotBuilder.AddCustomCommandCreator<TCreator>()
			 => WithBot(services => services.AddScoped<ICreateBotCommandHandler, TCreator>());

		public IBotUnitBuilder Apply<TDecorator>() where TDecorator : UnitItemSerializeDecorator
			=> WithUnit(services => services.AddScoped<IUnitItemDecorator, TDecorator>());

		#endregion
	}
}

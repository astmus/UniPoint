using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Abstractions;

using MissCore.Bot;
using MissCore.Data;
using MissCore.Presentation.Convert;

namespace MissCore.Response
{

	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record UnitActionResponse<T>(IHandleContext Context = default) : BaseResponse<BotAction<T>>(Context), IInteraction<T> where T : class
	{
		Message Message
			=> Context.Take<Message>();

		Func<JsonConverter> ConverterDelegate = Context.GetBotService<ActionSerializeConverter<BotAction<T>>>;
		public Message<BotAction<T>> CurrentMessage { get; protected set; }
		public override int Length
			=> Content?.ToString().Length ?? 0;
		protected override JsonConverter CustomConverter
			=> ConverterDelegate();

		[JsonProperty("text")]
		public override IUnitEntity Content { get; protected set; }

		public override async Task Commit(CancellationToken cancel)
		{
			if (Content == null) return;
			CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
			Content = default;
		}

		//public override IActionsSet Actions
		//    => Content.Actions; 

		public override IResponse<BotAction<T>> InputDataInteraction(string description, IActionsSet options = null)
		{
			Content = Unit<T>.Init(description);
			ActionsSet = options;
			return this;
		}

		public override IResponse CompleteInteraction(object completeObject)
		{
			Content = Unit<T>.Init(completeObject);
			if (Actions is IChatActionsSet set)
				ActionsSet = set.RemoveKeyboard();

			return this;
		}

		IUnitContainable<BotAction<T>> contentUnit;
		public override void InitializeCombinedContent()
		{
			ConverterDelegate = Context.GetBotService<CombinedUnitSerializeConverter<T>>;
			Content = contentUnit = Context.BotServices.Activate<ContentUnit<BotAction<T>>>();
		}

		public override void WriteUnit<TData>(TData unit)
		{
			ConverterDelegate = Context.GetBotService<ActionSerializeConverter<BotAction<T>>>;
			Content = unit;
		}

		IResponse IInteractiveResponse.InputDataInteraction(string description, IActionsSet options)
		{
			throw new NotImplementedException();
		}

		public override IResponse Write(object data)
		{
			Content = Unit<T>.Init(data);
			return this;
		}

		public override void AddUnit<TData>(IUnit<TData> unit)
			=> contentUnit.Add(unit);

		public override void WriteMetadata<TData>(TData meta)
		{
			throw new NotImplementedException();
		}

		public override void AddUnits<TData>(IEnumerable<TData> units)
		{
			throw new NotImplementedException();
		}
	}
	public record BotCommandResponse<T>(IHandleContext Context = default) : BaseResponse<T>(Context), IInteraction<T> where T : BaseAction
	{
		Message Message
			=> Context.Take<Message>();

		Func<JsonConverter> ConverterDelegate = Context.GetBotService<ActionSerializeConverter<T>>;
		public Message<T> CurrentMessage { get; protected set; }
		public override int Length
			=> Content?.ToString().Length ?? 0;
		protected override JsonConverter CustomConverter
			=> ConverterDelegate();

		public override IUnitEntity Content { get; protected set; }

		public override async Task Commit(CancellationToken cancel)
		{
			if (Content == null) return;
			CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
			Content = default;
		}

		//public override IActionsSet Actions
		//    => Content.Actions; 

		public override IResponse<T> InputDataInteraction(string description, IActionsSet options = null)
		{
			Content = Unit<T>.Init(description);
			ActionsSet = options;
			return this;
		}

		public override IResponse CompleteInteraction(object completeObject)
		{
			Content = Unit<T>.Init(completeObject);
			if (ActionsSet is IChatActionsSet set)
				ActionsSet = set.RemoveKeyboard();

			return this;
		}

		IUnitContainable<T> contentUnit;
		public override void InitializeCombinedContent()
		{
			ConverterDelegate = Context.GetBotService<CombinedUnitSerializeConverter<T>>;
			Content = contentUnit = Context.BotServices.Activate<ContentUnit<T>>();
		}

		public override void WriteUnit<TData>(TData unit)
		{
			ConverterDelegate = Context.GetBotService<ActionSerializeConverter<T>>;
			Content = unit;
		}

		IResponse IInteractiveResponse.InputDataInteraction(string description, IActionsSet options)
		{
			throw new NotImplementedException();
		}



		public override void AddUnit<TData>(IUnit<TData> unit)
			=> contentUnit.Add(unit);

		public override void WriteMetadata<TData>(TData meta)
		{
			throw new NotImplementedException();
		}

		public override void AddUnits<TData>(IEnumerable<TData> units)
		{
			throw new NotImplementedException();
		}

		public override IResponse Write(object data)
		{
			throw new NotImplementedException();
		}
	}
}



using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissCore.Actions;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Presentation.Convert;

using Newtonsoft.Json.Linq;

namespace MissCore.Response
{

	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record Response<TUnit>(IHandleContext Context = default) : BaseResponse<TUnit>(Context) where TUnit : class
	{
		Message Message
			=> Context.Take<Message>();

		Func<JsonConverter> ConverterDelegate = Context.GetBotService<UnitSerializeConverter<TUnit>>;
		public Message CurrentMessage { get; protected set; }

		public override int Length
			=> Content?.ToString().Length ?? 0;

		protected override JsonConverter CustomConverter
			=> ConverterDelegate();

		public override async Task Commit(CancellationToken cancel)
		{
			if (Content == null) return;

			if (Content is IInteractableUnit<TUnit> unit)
			{
				Actions = unit.Actions;
				unit.Actions = null;
			}
			CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
			Content = default;
		}

		public override IUnitEntity Content { get; protected set; }

		public override IResponse<TUnit> InputDataInteraction(string description, IActionsSet options = null)
		{
			ActionsSet = options;
			return this;
		}

		public override IResponse CompleteInteraction(object completeObject)
		{
			Content = DataUnit<TUnit>.Init(completeObject);
			if (Actions is IChatActionsSet set)
				ActionsSet = set.RemoveKeyboard();

			return this;
		}

		IUnitCollection<TUnit> contentUnits;
		public override void InitializeCombinedContent()
		{
			ConverterDelegate = Context.GetBotService<CombinedUnitSerializeConverter<TUnit>>;
			Content = contentUnits = Context.BotServices.Activate<DataUnit<TUnit>.UnitCollection>();
		}

		public override void WriteUnit<TData>(TData unit)
		{
			ConverterDelegate = Context.GetBotService<UnitSerializeConverter<TUnit>>;
			Content = unit;
		}


		public override IResponse Write<TData>(TData data)
		{
			Content = new ValueUnit(data);
			return this;
		}


		public override void AddUnit<TData>(IUnit<TData> unit)
		{
			InitializeCombinedContent();
			contentUnits.Add(unit);
		}

		public override void WriteMetadata<TData>(TData meta)
		{
			throw new NotImplementedException();
		}

		public override void AddUnits<TData>(IEnumerable<TData> units)
		{
			throw new NotImplementedException();
		}

		public override IResponse Write(IUnitItem data)
		{
			throw new NotImplementedException();
		}

		//public override void Add<TUnit>(TUnit unit) where TUnit: T
		//    => contentUnit.Add(unit);
	}
}



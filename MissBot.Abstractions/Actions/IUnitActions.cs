using MissBot.Abstractions.Converters;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions.Actions;

public interface IUnitActions<TUnit> : IUnitActionsSet
{
	[JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
	[JsonConverter(typeof(UnitActionsConverter), "inline_keyboard")]
	public IEnumerable<IEnumerable<IUnitAction<TUnit>>> Actions { get; init; }
}

public interface IChatActionsSet : IActionsSet
{
	public IActionsSet RemoveKeyboard();
}

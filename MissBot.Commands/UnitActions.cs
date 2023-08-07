using System.Diagnostics.CodeAnalysis;
using MissBot.Entities.Abstractions;

namespace MissBot.Entities;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public record UnitActions(
	[JsonProperty("inline_keyboard")]
    //[JsonConverter(typeof(UnitActionsConverter))]
    IEnumerable<IEnumerable<IUnitAction>> Actions) : IUnitActionsSet, IActionsSet
{
	public bool? Selective { get; set; }

	//[return: NotNullIfNotNull(nameof(action))]
	//public static implicit operator UnitActions(UnitAction? action)
	//	=> action is null ? default : new(action);

	[return: NotNullIfNotNull(nameof(actionName))]
	public static implicit operator UnitActions(string actionName)
		=> actionName is null ? default : new(actionName!);

	[return: NotNullIfNotNull(nameof(actionUnion))]
	public static implicit operator UnitActions(IEnumerable<IUnitAction>[] actionUnion)
		=> actionUnion is null ? default : new(actionUnion.ToList());

	[return: NotNullIfNotNull(nameof(actionUnion))]
	public static implicit operator UnitActions(IUnitAction[] actionUnion)
		=> actionUnion is null ? default : new(actionUnion);
}

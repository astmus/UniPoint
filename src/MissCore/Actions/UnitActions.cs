using System.Diagnostics.CodeAnalysis;

using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;

namespace MissCore.Actions;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public record UnitActions<TUnit>(
	[JsonProperty("inline_keyboard")]
    //[JsonConverter(typeof(UnitActionsConverter))]
    IEnumerable<IEnumerable<IUnitAction<TUnit>>> Actions) : IUnitActions<TUnit>, IUnitActionsSet, IActionsSet where TUnit : class
{
	[return: NotNullIfNotNull(nameof(action))]
	public static implicit operator UnitActions<TUnit>?(UnitAction? action)
		=> action is null ? default : new(action);

	[return: NotNullIfNotNull(nameof(actionName))]
	public static implicit operator UnitActions<TUnit>?(string? actionName)
		=> actionName is null ? default : new(actionName!);

	[return: NotNullIfNotNull(nameof(actionUnion))]
	public static implicit operator UnitActions<TUnit>?(IEnumerable<IUnitAction<TUnit>>[]? actionUnion)
		=> actionUnion is null ? default : new(actionUnion.ToList());

	[return: NotNullIfNotNull(nameof(actionUnion))]
	public static implicit operator UnitActions<TUnit>?(IUnitAction[]? actionUnion)
		=> actionUnion is null ? default : new(actionUnion);
}

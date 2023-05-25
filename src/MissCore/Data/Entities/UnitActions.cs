using System.Diagnostics.CodeAnalysis;
using MissBot.Abstractions.Actions;
using MissCore.Data.Entities;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class UnitActions : IActionsSet
{
    [JsonProperty("InlineKeyboard", Required = Required.Always)]
    public IEnumerable<IEnumerable<UnitAction>> Actions { get; }
    public UnitActions(UnitAction unitAction) : this(new[] { unitAction })
    { }
    public UnitActions(IEnumerable<UnitAction> unitActionsRow) : this(new[] { unitActionsRow })
    { }

    [JsonConstructor]
    public UnitActions(IEnumerable<IEnumerable<UnitAction>> actionsUnion)
        => Actions = actionsUnion;

    public static UnitActions Empty() =>
        new(Array.Empty<UnitAction[]>());

    [return: NotNullIfNotNull(nameof(action))]
    public static implicit operator UnitActions?(UnitAction? action)
        => action is null ? default : new(action);

    [return: NotNullIfNotNull(nameof(actionName))]
    public static implicit operator UnitActions?(string? actionName)
        => actionName is null ? default : new(actionName!);

    [return: NotNullIfNotNull(nameof(actionUnion))]
    public static implicit operator UnitActions?(IEnumerable<UnitAction>[]? actionUnion)
        => actionUnion is null ? default : new(actionUnion);

    [return: NotNullIfNotNull(nameof(actionUnion))]
    public static implicit operator UnitActions?(UnitAction[]? actionUnion)
        => actionUnion is null ? default : new(actionUnion);
}

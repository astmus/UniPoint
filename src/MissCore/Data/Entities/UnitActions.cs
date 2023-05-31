using System.Diagnostics.CodeAnalysis;
using MissBot.Abstractions.Actions;
using MissCore.Data.Entities;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class UnitActions : IActionsSet
{
    [JsonProperty("inline_keyboard", Required = Required.Always)]
    public IEnumerable<IEnumerable<UnitAction>> Actions { get; protected set; }
    public UnitActions(UnitAction unitAction) : this(new[] { unitAction })
    { }
    public UnitActions(IEnumerable<UnitAction> unitActionsRow) : this(new[] { unitActionsRow })
    {       
    }
    public UnitActions(int rowSize, IEnumerable<UnitAction> unitActionsRow) : this(unitActionsRow.Chunk(rowSize))
    { }
    [JsonConstructor]
    public UnitActions(IEnumerable<IEnumerable<UnitAction>> actionsUnion)
        => Actions = actionsUnion;

    public static UnitActions Empty() =>
        new(Array.Empty<UnitAction[]>());

    public void ClearActions()
    {
        Actions = null;
    }

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

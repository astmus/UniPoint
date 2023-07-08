namespace MissBot.Abstractions.Actions
{
	public interface IBotUnitRequest : IUnitRequest
	{
		// string Parameters { get; }
	}

	public interface IUnitRequest<in TUnit> : IUnitRequest
	{

	}

	public interface ISearchUnitRequest<in TUnit> : IUnitRequest<TUnit>
	{
		string Query { get; init; }
	}

	public interface IUnitRequest
	{
		IEnumerable<IUnitParameter> Params { get; }
		RequestOptions Options { get; set; }
		string GetCommand();
	}

	//public readonly record struct UnitParameter(string Name, object Value) : IUnitParameter
	//{
	//}

	[Flags]
	public enum RequestOptions : byte
	{
		Unknown = 0,
		JsonAuto = 1,
		JsonPath = 2,
		Scalar = 4,
		RootContent = 8
	}

	public interface IMetaUnit
	{
		IMetaData MetaData { get; }
	}
}

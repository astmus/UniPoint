using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace MissBot.Abstractions.Actions
{
	public interface IUnitRequest<in TUnit> : IUnitRequest
	{

	}

	public interface ISearchUnitRequest : IUnitRequest
	{
		string Query { get; set; }
	}

	public interface ISearchUnitRequest<in TUnit> : IUnitRequest<TUnit>, ISearchUnitRequest
	{
		void SetEtalone(TUnit unit);
	}

	public interface IUnitRequest
	{
		IEnumerable<IUnitParameter> Params { get; set; }
		string Get(params object[] args);
		string Options { get; set; }
		public static IUnitRequest operator +(IUnitRequest request, string append)
		{
			request.Options += append;
			return request;
		}
	}


	[Flags]
	[EnumExtensions]
	public enum RequestOptions : byte
	{
		Unknown = 0,
		[Description(" for json path")]
		JsonAuto = 1,
		[Description(" for json auto")]
		JsonPath = 2,
		[Description(" without_array_wrapper")]
		Scalar = 4,
		[Description(" root('Content')")]
		RootContent = 8
	}
}

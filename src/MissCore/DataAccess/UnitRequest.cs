using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;

using MissCore.Bot;

namespace MissCore.DataAccess
{
	public record UnitRequest : BotUnit, IUnitRequest
	{
		Lazy<FormattableUnit> LazyUnit;
		public static implicit operator string(UnitRequest cmd)
			=> cmd.Get();

		public UnitRequest(string cmd = default)
		{
			LazyUnit = new Lazy<FormattableUnit>(()
				=> FormattableUnit.Create(cmd ?? Format));
		}

		[JsonIgnore]
		public IEnumerable<IUnitParameter> Params { get; set; }
		public string Options { get; set; }

		public override string ToString()
			=> Get();

		public virtual string Get(params object[] args)
			=> LazyUnit.Value.GetCommand();


		public static UnitRequest operator +(UnitRequest request, string append)
		{
			request.Template += append;
			return request;
		}
	}

	public record UnitRequest<TUnit>(string raw = default) : UnitRequest(raw), IUnitRequest<TUnit> where TUnit : class
	{
		public static implicit operator string(UnitRequest<TUnit> cmd)
			=> cmd.Get();
		public static UnitRequest<TUnit> operator +(UnitRequest<TUnit> request, string append)
		{
			request.Template += append;
			return request;
		}
	}

	public record BotCommandUnitRequest<TUnit> : UnitRequest<TUnit>, IUnitRequest<TUnit> where TUnit : class
	{
		public static implicit operator string(BotCommandUnitRequest<TUnit> cmd)
			=> cmd.Get();
	}
}

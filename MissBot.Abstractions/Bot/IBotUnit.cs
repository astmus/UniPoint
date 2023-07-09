using MissBot.Abstractions.Converters;
using MissBot.Entities.Abstractions;
using MissBot.Extensions;

namespace MissBot.Abstractions.Bot
{
	public interface IInteractableUnit
	{
	}

	public interface IInteractableUnit<TUnit> : IUnit<TUnit>, IInteractableUnit where TUnit : class
	{
		[JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
		[JsonConverter(typeof(UnitActionsConverter), "inline_keyboard")]
		public IEnumerable<IEnumerable<IUnitAction<TUnit>>> Actions { get; set; }
	}

	public interface IIdentibleUnit<TUnit> where TUnit : BaseUnit
	{
		object Identifier { get; }
	}

	public interface IParameterizedUnit
	{
		string Parameters { get; }
		IEnumerable<string> GetParameters()
		{
			if (string.IsNullOrEmpty(Parameters)) return null;

			IList<string> parameters = null;
			var enumerator = Parameters.SplitParameters().GetEnumerator();

			if (enumerator.MoveNext())
			{
				parameters = new List<string>();
				parameters.Add(enumerator.Current);
			}
			else
				return Array.Empty<string>();

			while (enumerator.MoveNext())
				parameters.Add(enumerator.Current);

			return parameters;
		}
	}

	public interface IBotUnit : IBotEntity, ITemplatedUnit, IParameterizedUnit, IUnitEntity
	{
		string Description { get; set; }
	}

	public interface IBotUnit<TUnit> : IBotEntity, IUnitEntity, IDataUnit<TUnit> where TUnit : class
	{
		void SetContext<TDataUnit>(TDataUnit data) where TDataUnit : class, IUnit<TUnit>;
	}
}

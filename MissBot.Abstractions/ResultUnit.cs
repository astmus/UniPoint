
using System.Collections;

using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Results.Inline;
using MissBot.Identity;

using Newtonsoft.Json.Linq;

namespace MissCore.Data.Entities
{
	public abstract record ResultUnit<T> : BaseUnit<T>, IUnit<T>, IBotUnit<T> where T : class
	{
		public override object Identifier
			=> Id<T>.Instance.Value;
		public virtual string Entity { get; set; }
		public virtual string Title { get; set; } = Id<T>.Instance.Value;
		public abstract string Description { get; set; }

		public abstract ResultContent Content { get; }

		public T UnitData
			=> DataContext?.GetUnitEntity<T>();

		public IUnitContext DataContext { get; set; }
		public override IEnumerator UnitEntities
			=> DataContext.UnitEntities;

		public string Template { get; set; }
		public string Format { get; set; }
		public string Parameters { get; }

		public abstract void SetDataContext<TRoot>(TRoot data) where TRoot : JToken;

		void IBotUnit<T>.SetContext<TDataUnit>(TDataUnit data)
		{
			throw new NotImplementedException();
		}
	}
}


using System.Collections;
using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Results.Inline;
using MissBot.Identity;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Entities
{
	public abstract record ResultUnit<T> : BaseUnit, IUnit<T>, IBotUnit<T> where T : class
	{
		public override object Identifier
			=> Id<T>.Instance.Key;
		public abstract string Title { get; set; }
		public abstract string Description { get; set; }

		public abstract IInlineContent Content { get; }

		public MetaType ContentType
			=> MetaType.Unit;

		public T UnitData
			=> DataContext?.GetUnitEntity<T>();

		public abstract void SetContextRoot<TRoot>(TRoot data) where TRoot : JToken;
		public abstract void SetContext<TDataUnit>(TDataUnit data) where TDataUnit : class, IUnit<T>;

		public virtual string EntityKey { get; set; }
		public IUnitContext DataContext { get; set; }
		public override IEnumerator UnitEntities
			=> DataContext.UnitEntities;
	}
}

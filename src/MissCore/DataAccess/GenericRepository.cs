using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using MissCore.Bot;
using MissCore.Data;
using MissBot.Abstractions.Actions;
using System.Runtime.CompilerServices;
using LinqToDB;
using System.Collections;
using Microsoft.Extensions.Options;
using MissCore.Storage;

namespace MissCore.DataAccess
{
	[Table("##BotUnits")]
	public class GenericRepository<TUnit> : DataContext, IRepository<TUnit>, IUnitEntity where TUnit : class
	{
		public GenericRepository(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
		{
			int i = 9;
		}


		public Task<IEnumerable<TUnit>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TUnit> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<TEntityType> GetAsync<TEntityType>() where TEntityType : TUnit
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : TUnit
		{
			throw new NotImplementedException();
		}


		[Column]
		public virtual string Unit { get; set; } = Id<TUnit>.Instance.Value;

		public string Description { get; set; }
		[Column]
		public virtual string Entity { get; set; }
		[Column]
		public virtual string Template { get; set; }

		public string Format
		=> string.Format(Template, Identifier);

		[Column]
		public string Parameters { get; set; }

		public object Identifier { get; }
	}
}


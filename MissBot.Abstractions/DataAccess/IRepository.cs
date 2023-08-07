using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Configuration;

namespace MissBot.Abstractions.DataAccess
{

	public delegate IQueryable<TEntity> Executor<TEntity>(FormattableString s);
	public interface IQueryContext
	{
		string Get(params object[] args);
	}
	public interface IQueryUnit<TData> : IUnitRequest<TData>
	{
		string Get(params object[] args);
	}
	public interface IRepository<TEntity>
	{
		Task<IEnumerable<TEntity>> GetAllAsync();
		IEnumerable<TEntity> GetAll();
		Task<TEntityType> GetAsync<TEntityType>() where TEntityType : TEntity;
		IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : TEntity;
	}

	public interface IBotCommandsRepository : IRepository<BotCommand>
	{
		IList<BotCommand> Commands { get; }
		void AddCommand<TCommand>(TCommand command) where TCommand : BotCommand;
		Task<bool> Commit(CancellationToken cancel = default);
	}
}

using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.DataAccess
{
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

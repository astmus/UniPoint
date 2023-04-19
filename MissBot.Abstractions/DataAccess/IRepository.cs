using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions.DataAccess
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetAll();
        Task<TEntityType> GetAsync<TEntityType>() where TEntityType : TEntity;
        IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : TEntity;
        Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : TEntity;
    }

    public interface IBotCommandsRepository : IRepository<BotCommand>
    {
        TCommand GetByName<TCommand>(string name) where TCommand : BotCommand;
    }
}

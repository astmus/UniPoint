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
        IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : TEntity;
        Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType: TEntity;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions.DataAccess
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> GetAsyncForAction<TAction>(TAction action) where TAction: IEntityAction<TEntity>;
    }
}

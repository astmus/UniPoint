using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions.Persistance
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> GetAsyncById(int id);
    }
}

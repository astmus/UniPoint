using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;

namespace MissBot.DataAccess
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    {
        public abstract Task<TEntity> GetAsyncForAction<TAction>(TAction action) where TAction : IEntityAction<TEntity>;
    }
}

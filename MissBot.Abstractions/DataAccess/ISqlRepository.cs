using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public interface ISqlRepository : IRepository
    {
        Task ExecuteCommandAsync(SQLUnit sql, CancellationToken cancel = default);
        Task<TResult> HandleSqlQueryAsync<TResult>(ISQLUnit sql, CancellationToken cancel = default) where TResult:class;
    }
   

    public interface IRepository
    { }
}

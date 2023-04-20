using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.DataAccess.Sql;

namespace MissBot.Abstractions.DataAccess
{
    public interface ISqlRepository : IRepository
    {
        Task ExecuteCommandAsync(SQL sql, CancellationToken cancel = default);
        Task<TScalar> HandleScalarQueryAsync<TScalar>(SQL sql, CancellationToken cancel = default) where TScalar:class;
    }
   

    public interface IRepository
    { }
}

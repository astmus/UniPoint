using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions.DataAccess;

namespace MissBot.DataAccess.Interfacet
{
    public interface ISqlHandler
    {
        Task ExecuteCommandAsync(SQL sql, CancellationToken cancel = default);
        Task<TScalar> HandleQueryAsync<TScalar>(SQL sql, CancellationToken cancel = default) where TScalar : class;
    }
}

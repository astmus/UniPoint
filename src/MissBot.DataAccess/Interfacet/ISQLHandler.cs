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
        Task<int> HandleSqlCommandAsync(ISQLUnit sql, CancellationToken cancel = default);
    }
}

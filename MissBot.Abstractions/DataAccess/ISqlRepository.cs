using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface ISqlRepository : IRepository
    {
        Task ExecuteCommandAsync(string sql, CancellationToken cancel = default);
        Task<TScalar> HandleScalarQueryAsync<TScalar>(string sql, CancellationToken cancel = default) where TScalar:class;
    }
   

    public interface IRepository
    { }


    public interface IJsonRepository : IRepository
    {        
        Task<TResult> HandleQueryAsync<TResult>(string sql, CancellationToken cancel = default) where TResult: class;
        Task<IList<TResult>> HandleQueryItemsAsync<TResult>(string sql, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(string sql, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(string sql, CancellationToken cancel = default);
    }
}

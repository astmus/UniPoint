
using System.Runtime.CompilerServices;
using System.Text.Json;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;
using Newtonsoft.Json;

namespace MissDataMaiden.Queries
{
    public record Filter(int skip, int take, string predicat);
    public record SingleRequest<TEntity>(string sql, string connectionString) :  IRequest<TEntity>;
    public record SqlQuery<TEntity>(string sql = default, string connectionString = default, Filter filter = default) : IRequest<IEnumerable<TEntity>> where TEntity : class
    {
        public record Request(string sql, string connectionString) : SqlQuery<TEntity>(sql, connectionString), IRequest<TEntity>;

        public static readonly TEntity Sample = Activator.CreateInstance<TEntity>();
        public static readonly SqlQuery<TEntity>.Request Instance = new Request("", "");
        public virtual async Task<TUnit> SelectAsync<TUnit>(string ActionName, string connection,  CancellationToken cancellationToken = default) where TUnit : Unit<TEntity>
        {        
            var instance = new SqlQuery<TUnit>.Request($"Select * from ##BotActionPayloads where EntityAction = '{ActionName}' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER", connection);
            return await instance.LoadAsync();
        }
        public virtual async Task<TEntity> LoadAsync(CancellationToken cancellationToken = default)
        {            
                TEntity result = default(TEntity);
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = sql;
                    if (filter is Filter predicate)
                        query = string.Format(sql, predicate.skip, predicate.take, predicate.predicat);
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        await conn.OpenAsync(cancellationToken).ConfigFalse();

                        var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                        if (!reader.HasRows)
                            return result;
                        else
                            while (await reader.ReadAsync())
                            {
                                var str = reader.GetString(0);
                                return JsonConvert.DeserializeObject<TEntity>(str);
                            }
                    }
                    return result;
                }
            }
        public virtual async Task<IEnumerable<TEntity>> Handle(CancellationToken cancellationToken = default) 
        {
            IEnumerable<TEntity> result = Enumerable.Empty<TEntity>();
            using (var conn = new SqlConnection(connectionString))
            {
                string query = sql;
                if (filter is Filter predicate)
                   query = string.Format(sql, predicate.skip, predicate.take, predicate.predicat);
                using (var cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync(cancellationToken).ConfigFalse();

                    var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                    if (!reader.HasRows)
                        return result;
                    else
                        while (await reader.ReadAsync())
                        {
                            var str = reader.GetString(0);
                            return JsonConvert.DeserializeObject<List<TEntity>>(str);
                        }
                }
                return result;
            }


        }
    }
}

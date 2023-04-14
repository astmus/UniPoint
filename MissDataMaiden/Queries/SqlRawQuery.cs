
using System.Runtime.CompilerServices;
using System.Text.Json;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;
using Newtonsoft.Json;

namespace MissDataMaiden.Queries
{
    public record SqlQuery<TEntity>(string sql = default, string connectionString = default) : IRequest<IEnumerable<TEntity>> where TEntity : class
    {
        public record Query(string sql, int skip, int take, string filter);
        public record Request(string sql, string connectionString) : SqlQuery<TEntity>(sql, connectionString);

        public static readonly TEntity Sample = Activator.CreateInstance<TEntity>();
        public static readonly SqlQuery<TEntity>.Request Instance = new Request("", "");

        public virtual async Task<IEnumerable<TEntity>> Handle(CancellationToken cancellationToken = default) 
        {
            IEnumerable<TEntity> result = Enumerable.Empty<TEntity>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
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

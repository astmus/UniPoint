//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using LinqToDB;
//using MissBot.Abstractions;
//using MissBot.Abstractions.Actions;
//using MissBot.Abstractions.DataAccess;
//using MissBot.DataAccess.Interfacet;
//using Newtonsoft.Json;

//namespace MissBot.DataAccess.Sql
//{
//    public abstract class BotContext
//    {
//        public BotContext(BotDataContext Context)
//        {
//            this.Context = Context;
//        }

//        public BotDataContext Context { get; }

//        //  protected abstract BotContext(BotContextOptions ctxOptions) 
//        // internal record ContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);

//        internal virtual BotContextOptions BotContextOptions
//            => new BotContextOptions(Context.ContextOptions.connectionString);

//        public async Task ExecuteCommandAsync(SQLCommand sql, CancellationToken cancel = default)
//            => await ExecuteCommandAsync(sql.Command, cancel);

//        public async Task ExecuteCommandAsync(string sql, CancellationToken cancel = default)
//        {
//            using (var connection = Context.DataProvider.CreateConnection(Context.ContextOptions.connectionString))
//            {
//                await connection.OpenAsync();
//                using (var cmd = connection.CreateCommand())
//                {
//                    cmd.CommandText = sql;
//                    await cmd.ExecuteNonQueryAsync(cancel).ConfigureAwait(false);
//                }
//                await connection.CloseAsync();
//            }
//        }


//        public virtual async Task<TResult> HandleQueryAsync<TResult>(BotRequest sql, CancellationToken cancel = default) where TResult : class
//        {
//            TResult result = default(TResult);
//            try
//            {
//                using (var connection = Context.DataProvider.CreateConnection(Context.ContextOptions.connectionString))
//                {
//                    await connection.OpenAsync();
//                    using (var cmd = connection.CreateCommand())
//                    {
//                        cmd.CommandText = sql.Command;
//                        if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
//                            result = JsonConvert.DeserializeObject<TResult>(res);
//                    }
//                    await connection.CloseAsync();
//                }
//            }
//            catch (Exception error)
//            {
//              //  sql.Result = new SQLResult(Convert.ToUInt32(result), error.HResult, error.Message);
//            }         
//            return result;
//        }        
//    }





//}

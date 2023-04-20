using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Attributes;
using MissBot.DataAccess.Sql;
using MissBot.Extensions.Entities;
using MissBot.Handlers;
using MissCore.Bot;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using MissDataMaiden.Entities;
using MissDataMaiden.Queries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;

namespace MissDataMaiden
{
    internal class MissDataAnswerHandler : InlineAnswerHandler
    {
        private readonly IMediator mediator;
        private readonly IConfiguration config;
        IMediator mm;
        IJsonRepository repository;
        public MissDataAnswerHandler(IJsonRepository jsonRepository, IConfiguration config)
        {
            repository = jsonRepository;
            this.config = config;
        }

        //protected Task HandleAsync(IHandleContext context, string command, string[] args) => command switch
        //{
        //    nameof(DBInfo) => HandleAsync<DBInfo>(context, args),
        //    nameof(DBDelete) => HandleAsync<DBDelete>(context, args),
        //    nameof(DBRestore) => HandleAsync<DBRestore>(context, args),
        //    _ => context.Get<AsyncHandler>()(context)
        //};      
        public record DataBaseActionQuery(string sql, string connectionString) : SingleRequest<JArray>(sql, connectionString);

        public class DataBaseActionQueryHandler : IRequestHandler<DataBaseActionQuery, JArray>
        {
            public async Task<JArray> Handle(DataBaseActionQuery request, CancellationToken cancellationToken)
            {
                //InlineEntityAction<DataBase> result = default(InlineEntityAction<DataBase>);
                using (var conn = new SqlConnection(request.connectionString))
                {
                    List<JObject> res = new List<JObject>();
                    using (var cmd = new SqlCommand(request.sql, conn))
                    {
                        await conn.OpenAsync(cancellationToken).ConfigFalse();

                        var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                        if (!reader.HasRows)
                            return null;
                        else
                            while (await reader.ReadAsync())
                            {
                                var str = reader.GetString(0);
                                res.Add(JObject.Parse(str));

                                //return JsonConvert.DeserializeObject<List<InlineEntityAction<DataBase>>>(str);
                            }
                    }
                    return JArray.FromObject(res);

                }
            }
        }

        record DataBaseRequest : SQLQuery<DataBase>
        {            
            public override string CreateCommand<TEntity>()
                => $"select * from ##Info where Id = {Entity.Id}";
            
            //public override Cmd<DataBase> Query { get => this with { cmd = string.Format(Template, Param?.Id ?? sample?.Id) }; }
            
        }

        public override async Task HandleResultAsync(ChosenInlineResult result, IContext<ChosenInlineResult> context)
        {
            int id = result.Query.Length > 0 ? int.Parse(result.ResultId.Replace(result.Query, "")) : int.Parse(result.ResultId);
            var request = Unit<DataBaseRequest>.Sample with { Entity = Unit<DataBase>.Sample with { Id = "6" } };

            var dbinco = await repository.HandleQueryGenericObjectAsync(request.Request);

            var unit = Unit<ChosenInlineResult>.Meta; //  ValueUnit.Parse(dbinco);
            var response = context.CreateResponse(result);
            response.WriteMetadata(unit);
            await response.Commit(default);
            //request.Tempalted(Convert.ToInt32(result.ResultId).ToString()).Cmd);

            //var search = await  repository.HandleQueryGenericObjectAsync(dbinco.WithCondition(id));
            //var action = await request.SelectAsync<InlineEntityAction<DataBase>>(", ConnectionString);
            //var inlineUnit = new DataBaseActionQuery (
            //result.Query.Length > 0 && result.ResultId.Contains(result.Query) ? string.Format(action.Payload, result.ResultId.Replace(result.Query, "")) : string.Format(action.Payload, result.ResultId), ConnectionString);
            //var payload = await mediator.Send(inlineUnit);
            await Task.CompletedTask;
            int i = 0;
            //var ctx = context.CreateDataContext<TAction>();
            //ctx.Data = Unit<TAction>.Sample with { Id = args[0] };

            //var handler = context.GetAsyncHandler<TAction>();
            //try
            //{
            //    await handler.HandleAsync(ctx);
            //}
            //catch (Exception ex)
            //{
            //    await notifier.SendTextAsync(ex.Message);
            //}
        }
    }
}


using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Handlers;
using MissCore.Bot;
using MissCore.Entities;
using MissDataMaiden.Commands;
using MissDataMaiden.Entities;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace MissDataMaiden
{
    internal class MissDataAnswerHandler : InlineAnswerHandler
    {

        private readonly IConfiguration config;

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
        
        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        record DataBaseRequest : Search<DataBase>
        {
            public DataBaseRequest()
            {
                
            }
            public string Id { get; set; }
            public string Info { get; set; }
            public string DBName { get; set; }
            public string Status { get; set; }
            public string State { get; set; }
            public int DataFiles { get; set; }
            public int DataMB { get; set; }
            public int LogFiles { get; set; }
            public int LogMB { get; set; }
            public string RecoveryModel { get; set; }
            public string Created { get; set; }
            public string LastBackup { get; set; }
            public string IsReadOnly { get; set; }

            public List<EntityAction<DataBase>> Commands { get; set; }
            public static SQLCommand GetCommand(params object[] args)
                => string.Format($"SELECT * FROM ##Info a  INNER JOIN ##BotActions Commands ON Commands.Entity = a.Info WHERE a.Id = {{0}}", args);
            //public SQLCommand GetCommand()
            //    => string.Format($"SELECT * FROM ##Info a  INNER JOIN ##BotActions Commands ON Commands.Entity = a.Info WHERE a.Id = {{0}}", 6);

            //public override Cmd<DataBase> Query { get => this with { cmd = string.Format(Template, Param?.Id ?? sample?.Id) }; }
            public SQLCommand ToQuery(Func<DataBase, string> init)
                     => init(Entity);
        }
        //public static SQL<TEntity> EntityActions<TEntity>(string commandName) where TEntity : EntityAction<DataBase>
        //   => new  SQL<TEntity>( d=> 
        //            $"SELECT * FROM ##{nameof(BotAction)} WHERE Entity = '{nameof(DataBase)}' AND Command = '{commandName}' ");
        public override async Task HandleResultAsync(ChosenInlineResult result, IContext<ChosenInlineResult> context)
        {
            int id = result.Query.Length > 0 ? int.Parse(result.ResultId.Replace(result.Query, "")) : int.Parse(result.ResultId);
            //var request = Unit<DataBaseRequest>.Sample with { Entity = Unit<DataBase>.Sample with { Id = id.ToString() } };
            //Unit<DataBase>.Sample.Id = id.ToString();
                
            //var dbinco = await repository.HandleQueryItemsAsync<DataBase>(DataBaseRequest.GetCommand(id));

            var unit = Unit<DataBase>.Meta;

            //var detailsRequest =  EntityActions<DBDetails>(nameof(DBAction.Restore));
            //DataBaseRequest.Unit.

            var sql = new DataBaseRequest();//f
            //       => string.Format($"SELECT * FROM ##Info a  INNER JOIN ##BotActions Commands ON Commands.Entity = a.Info WHERE a.Id = {{0}}", 6));
            var template = sql.Command;
            template.Type = SQLType.JSONPath;
            
            //sql.ContentPropertyName = nameof(DataBase.Detail);
            var details =  await repository.HandleQueryAsync<DataBaseRequest>(sql);
            var response = context.CreateResponse(result);
            response.WriteMetadata(unit with { Data = Unit <DataBaseDetail>.Parse(unit) } );
           // response.Write(details);
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


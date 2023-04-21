using MissBot.Attributes;
using MissBot.Abstractions;
using MissDataMaiden.Queries;
using Duende.IdentityServer.Services;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MissCore.Entities;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using MediatR;
using MissBot.Common;
using MissBot.Extensions.Response;
using MissCore.Handlers;
using MissDataMaiden.Entities;
using MissBot.Abstractions.DataAccess;

namespace MissDataMaiden.Commands
{
    public record DBRestore : EntityAction<DataBase>;
    public record DBDelete : EntityAction<DataBase>;
    public record DBInfo : EntityAction<DataBase>;
    public record DataBaseDetail(string DBName, string Status, string State, int DataFiles, int DataMB,
                                            [JsonProperty] int LogFiles, int LogMB, string RecoveryModel, string Created, string LastBackup, string IsReadOnly,
                                            [JsonProperty] List<EntityAction<DataBase>> Commands
    ) : EntityAction<DataBase>
    {
        public DataBaseDetail() : this(default,default, default, default, default, default, default, default, default, default, default,default)        {
            
        }
        public DataBaseDetail(DataBaseDetail d) : base (d)
        {

        }
    }

    public class DdActionHandler : BaseHandler<InlineEntityAction<DataBase>>, IAsyncHandler<DBDelete>, IAsyncHandler<DBRestore>, IAsyncEntityActionHandler<DBInfo>
    {
        

        public Task HandleActionAsync(DBInfo action, IHandleContext context, CancellationToken cancel = default)
        {
           
            throw new NotImplementedException();
        }

        public async  Task HandleAsync(IContext<DBInfo> context)
        {
            var response = context.CreateResponse();
            
            //response.Write(new DBInfo.Response("0"));

            await response.Commit(default);
        }

        public Task HandleAsync(IContext<DBDelete> context)
        {
            throw new NotImplementedException();
        }

        public Task HandleAsync(IContext<DBRestore> context)
        {
            throw new NotImplementedException();
        }

        public override Task HandleAsync(IContext<InlineEntityAction<DataBase>> context)
        {
            throw new NotImplementedException();
        }
    }
    
}

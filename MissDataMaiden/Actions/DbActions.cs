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
                                            int LogFiles, int LogMB, string RecoveryModel, string Created, string LastBackup, string IsReadOnly,
                                            List<EntityAction<DataBase>> Commands
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

    //public class DiskCommandHandler : BotCommandHandler<Disk>
    //{
    //    private readonly IConfiguration config;
    //    SqlRaw<Disk.DataUnit>.Query CurrentRequest;        

    //    public DiskCommandHandler(IConfiguration config)
    //    {
    //        //var disk = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[1].Get<Disk>();
            
    //        this.config = config;
    //    }

    //    public override Disk Command
    //        => config.GetSection(nameof(IBotCommandInfo)).GetChildren().First().Get<Disk>();

    //    //public override async  Task BeforeComamandHandle(IContext<Disk> context)
    //    //{
    //    //    Disk.CommandResult response = context.Scope.Result;

    //    //    await response.SendHandlingStart(); 
    //    //}

    //    public override async Task RunAsync(Disk command, IContext<Disk> context)
    //    {
    //        IResponse response = context.Response;
  
    //        var mm = context.Root.BotServices.GetService<IMediator>();
  
    //        var result = response.Create(command);

    //        await foreach (var obj in mm.CreateStream(new Disk.Query(command.Payload)))
    //        {
    //            //context.Data.Result.Write(obj);
    //            result.Write(obj);
    //        }
    //        await result.Commit(default);            
    //    }     

    //    //public override BotCommand<Disk> GetDataForHandle()
    //    //    => new BotCommand<Disk>() { Command = Context.Data.Get<Message>().Command };

    //    //public override async Task HandleCommandAsync(DiskCommand command, string[] args)
    //    //{

    //    //	var mm = Context.Current.Get<IMediator>();
    //    //	var diskInfo = await mm.Send(CurrentRequest).ConfigFalse();
    //    //	var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(diskInfo);

    //    //	var str = result.Select(d =>
    //    //	{
    //    //		var header =
    //    //		d.Select(kv => $"{kv.Key.PadRight(6)[0..6].AsBTag()}").Aggregate($"", (s1, s2)
    //    //			=> $"{s1} {s2}").AsPreTag();
    //    //		var body =
    //    //		d.Select(kv => $"{kv.Value.PadRight(6)[0..6]}").Aggregate($"", (s1, s2)
    //    //			=> $"{s1} {s2}").AsPreTag();

    //    //		return new KeyValuePair<string, string>(header, body);
    //    //	}
    //    //	).GroupBy(gb => gb.Key, v => v.Value, (k, g) => new { Key = k, Items = string.Join("", g) })
    //    //	.Select(f => $"{f.Key}{f.Items}");
    //    //	var channel = Context.Client.Channel;
    //    //	await channel.WriteAsync(str).ConfigFalse();
    //    //	//, MissBot.Common.Types.Enums.ParseMode.Html, cancellationToken: CancellationToken.None
    //    //	//ParseMode.Markdown,
    //    //	//replyToMessageId: msg.MessageId,
    //    //	//replyMarkup: new InlineKeyboardMarkup(
    //    //	//    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
    //    //	//)
    //    //	return;
    //    //}
    //}
}

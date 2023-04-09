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

namespace MissDataMaiden.Commands
{

    public record Disk :  BotCommand<Disk.DataUnit>
    {
        [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public record DataUnit : Unit
        {
            public string Name { get; set; }
            public string Created { get; set; }
            public int DaysAgo { get; set; }
            public double Size { get; set; }
        }

        public record Query(string sql) : SqlRaw<DataUnit>.Query(sql);
        public class Handler : SqlRaw<DataUnit>.Handler<Query>
        {
            public Handler(IConfiguration config) : base(config)
            {
            }
        }
    }

    public class DiskCommandHandler : BotCommandHandler<Disk>
    {
        private readonly IConfiguration config;
        SqlRaw<Disk.DataUnit>.Query CurrentRequest;        

        public DiskCommandHandler(IConfiguration config)
        {
            //var disk = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[1].Get<Disk>();
            
            this.config = config;
        }

        public override Disk Command
            => config.GetSection(nameof(IBotCommandInfo)).GetChildren().First().Get<Disk>();



        //public override async  Task BeforeComamandHandle(IContext<Disk> context)
        //{
        //    Disk.CommandResult response = context.Scope.Result;

        //    await response.SendHandlingStart(); 
        //}

        public override async Task RunAsync(Disk command, IContext<Disk> context)
        {

            IResponse response = context.Response;

            await response.SendHandlingStart();
            
            //context.Data.Result.Write(new Disk.DataUnit() { });
            
            //var srv = context.BotServices .Get<IBotServicesProvider>();
            var mm = context.BotServices.GetService<IMediator>();
            //Disk.Result result = context.CreateResponse<Disk.Result>(null);
          //  context.Response = new Disk.Result();
            
            await foreach (var obj in mm.CreateStream(new Disk.Query(command.Payload)))
            {
                //context.Data.Result.Write(obj);
                command.Result.Write(obj);
            }

            await response.WriteAsync(command.Result, default);
                
        }     

        //public override BotCommand<Disk> GetDataForHandle()
        //    => new BotCommand<Disk>() { Command = Context.Data.Get<Message>().Command };

        //public override async Task HandleCommandAsync(DiskCommand command, string[] args)
        //{

        //	var mm = Context.Current.Get<IMediator>();
        //	var diskInfo = await mm.Send(CurrentRequest).ConfigFalse();
        //	var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(diskInfo);

        //	var str = result.Select(d =>
        //	{
        //		var header =
        //		d.Select(kv => $"{kv.Key.PadRight(6)[0..6].AsBTag()}").Aggregate($"", (s1, s2)
        //			=> $"{s1} {s2}").AsPreTag();
        //		var body =
        //		d.Select(kv => $"{kv.Value.PadRight(6)[0..6]}").Aggregate($"", (s1, s2)
        //			=> $"{s1} {s2}").AsPreTag();

        //		return new KeyValuePair<string, string>(header, body);
        //	}
        //	).GroupBy(gb => gb.Key, v => v.Value, (k, g) => new { Key = k, Items = string.Join("", g) })
        //	.Select(f => $"{f.Key}{f.Items}");
        //	var channel = Context.Client.Channel;
        //	await channel.WriteAsync(str).ConfigFalse();
        //	//, MissBot.Common.Types.Enums.ParseMode.Html, cancellationToken: CancellationToken.None
        //	//ParseMode.Markdown,
        //	//replyToMessageId: msg.MessageId,
        //	//replyMarkup: new InlineKeyboardMarkup(
        //	//    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
        //	//)
        //	return;
        //}
    }
}

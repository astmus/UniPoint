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

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public record Disk :  BotCommand<Disk>, IBotCommand
    {
        [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public record DataUnit : Unit<Disk>
        {
            public string Name { get; set; }
            public string Created { get; set; }
            public int DaysAgo { get; set; }
            public double Size { get; set; }
        }        

        public record Query(string sql) : SqlRaw<DataUnit>.Query(sql);
        public class Handler : SqlRaw<DataUnit>.StreamHandler<Query>
        {
            public Handler(IConfiguration config) : base(config)
            {
            }
        }
    }

    public record DiskResponse : Response<Disk>
    {        
        protected override Response<Disk> WriteUnit(Unit<Disk> unit) => unit switch
        {
            Disk.DataUnit du => WriteDataUnit(du),
            _ => base.WriteUnit(unit)
        };

        Response<Disk> WriteDataUnit(Disk.DataUnit data)
        {
            
            Text += $"{data.Name.Shrink(10)}       {data.Created}      {data.DaysAgo}      {data.Size}".AsCodeTag().LineTag();
            
            return this;
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
            => new Disk() { Payload = config.GetSection(nameof(IBotCommandInfo)).GetChildren().First().GetValue<string>("Payload") };

        //public override async  Task BeforeComamandHandle(IContext<Disk> context)
        //{
        //    Disk.CommandResult response = context.Scope.Result;

        //    await response.SendHandlingStart(); 
        //}

        public override async Task RunAsync(Disk command, IContext<Disk> context)
        {
            IResponse<Disk> response = context.CreateResponse(command);
            //command.Add(new Disk.DataUnit());
            var mm = context.Root.BotServices.GetService<IMediator>();
  
            

            await foreach (var obj in mm.CreateStream(new Disk.Query(command.Payload)))
            {
             
                response.Write(obj);
            }
            await response.Commit(default);            
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

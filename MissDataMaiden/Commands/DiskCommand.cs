using MissBot.Attributes;
using MissBot.Handlers;
using MissBot.Abstractions;
using MissDataMaiden.Queries;
using Duende.IdentityServer.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissDataMaiden.Commands
{

    public record Disk :  IBotCommandData
    {
        public string Payload { get; set; }
        public string[] Params { get; set; }
        public string Name { get; set; }       
    }

    public class DiskCommandHandler : BotCommandHandler<Disk>
    {
        SqlRawQuery CurrentRequest;        

        public DiskCommandHandler(IConfiguration config)
        {
            var disk = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[1].Get<Disk>();
            
            CurrentRequest = new SqlRawQuery(disk.Payload);
        }
        
        public override Disk Model { get; set; }

        public override async Task BeforeComamandHandle(IContext<Disk> context)
        {
            await context.Response.SendHandlingStart(); 
        }
        public override async Task HandleAsync(IContext<Disk> context, Disk data)
        {
            Model = data;
            var res = await
                context.Response.WriteMessageAsync(this, default);
                
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

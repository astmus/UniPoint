using System.ComponentModel;
using MissBot.Common;
using MissBot.Handlers;
using MissBot.Interfaces;
using MissCore.Abstractions;
using MissDataMaiden.Queries;
using Telegram.Bot.Types;

namespace MissDataMaiden.Commands
{
    [Description("Descirption from attribute")]
    public class Disk : BotCommand, IBotCommandData
    {
        public MissCommand<Disk> CommandData { get; set; }
        public string Payload { get; init; }
        public string[] Params { get; init; }
    }

    public class DiskCommandHandler : BotCommandHandler<Disk>
    {
        SqlRawQuery CurrentRequest;

        public override bool ItCanBeHandled(IHandleContext context)
            => base.ItCanBeHandled(context) && context.Data.Get<Message>().Text == nameof(Disk);

        public DiskCommandHandler(IConfiguration config)
        {
            //CurrentRequest = new SqlRawQuery(config.GetCommandByName(nameof(Disk)).Payload);
        }

        public override Task StartHandleAsync(Disk data, IHandleContext context)
            => throw new NotImplementedException();

        public override Task HandleCommandAsync(Disk command, string[] args)
        {
            Console.WriteLine($"Unhandled command {command}");
            return Task.CompletedTask;
        }

        public override Disk GetDataForHandle()
        {
            throw new NotImplementedException();
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

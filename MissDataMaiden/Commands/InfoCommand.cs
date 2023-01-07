using MediatR;
using MissBot.Common;
using MissBot.Handlers;
using MissBot.Interfaces;
using MissCore.Abstractions;
using MissDataMaiden.Queries;
using Telegram.Bot.Types;

namespace MissDataMaiden.Commands
{
    public class Info : BotCommand, IBotCommandData
    {
        public MissCommand<Info> CommandData { get; set; }
        public string Payload { get; init; }
        public string[] Params { get; init; }
    }

    internal class InfoCommandHadler : BotCommandHandler<Info>
    {

        SqlRawQuery CurrentRequest { get; set; }
   
        public IConfiguration Config { get; }

        IMediator mm;
        string connectionString;
        private const string CFG_KEY_COMMAND = nameof(IBotCommandInfo.Command);
        private const string CFG_KEY_DESCRIPTION = nameof(IBotCommandInfo.Description);
        private const string CFG_KEY_PARAMS = nameof(IBotCommandData.Params);
        public InfoCommandHadler(IBot controller, IConfiguration config)
        {
           
            Config = config;
            
        }
        #region Commented
        //public override async Task HandleCommandAsync(Info command, string[] args)
        //{
        //	mm = Context.Current.Get<IMediator>();

        //	var outChannel = Context.Client.Channel;

        //	var message = await outChannel.CreateAsync<DataBaseItem>(default, new DataBaseItem()).ConfigFalse();
        //	DataBaseItem item = message.Content[0];
        //	item.DaysAgo += 1;
        //	await outChannel.WriteAsync(message.With(item));
        //	await Task.Delay(1000);
        //	item.DaysAgo += 1;
        //	await outChannel.WriteAsync(message.With(item));
        //	await Task.Delay(1000);
        //	item.DaysAgo += 1;
        //	await outChannel.WriteAsync(message.With(item));
        //	await Task.Delay(1000);
        //	var data = await mm.Send(CurrentRequest).ConfigFalse();
        //	var items = JsonConvert.DeserializeObject<DataBaseItem[]>(data);			
        //	await outChannel.WriteAsync(message.With(items));
        //	//(
        //	//  Current.ChatId,
        //	//  "Loading", ParseMode.Html//,
        //	//									  //replyToMessageId: msg.MessageId,
        //	//									  //replyMarkup: new InlineKeyboardMarkup(
        //	//									  //    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
        //	//									  //)
        //	// );

        //	//	await Current.Client.EditMessageTextAsync(remoteResult.Chat.Id, remoteResult.MessageId, remoteResult.Text = "...");
        //	//var jsonData = await Handle(CurrentRequest, default);
        //	//var jItems = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonData);

        //	//var str = jItems.Select(d =>
        //	//{
        //	//	var header =
        //	//	d.Select(kv => $"{kv.Key.PadRight(6)[0..6].AsBTag()}").Aggregate($"", (s1, s2)
        //	//		=> $"{s1} {s2}").AsPreTag();
        //	//	var body =
        //	//	d.Select(kv => $"{kv.Value.PadRight(6)[0..6]}").Aggregate($"", (s1, s2)
        //	//		=> $"{s1} {s2}").LineTag().AsPreTag();

        //	//	return new KeyValuePair<string, string>(header, body);
        //	//})
        //	//.GroupBy(gb => gb.Key, v => v.Value, (k, g) => new { Key = k, Items = string.Join("", g) })
        //	//.Select(f => $"{f.Key}{f.Items}");
        //	//await Current.Client.EditMessageTextAsync(remoteResult.Chat.Id, remoteResult.MessageId,  remoteResult.Text = string.Join("\n",str), ParseMode.Html);

        //	//remoteResult.Text = "";
        //	//var dbItems = JArray.Parse(jsonData);
        //	//foreach (var item in dbItems.Children<JObject>())
        //	//{
        //	//	foreach (var prop in item.Properties())
        //	//		remoteResult.Text += $"{((string?)prop.Value).AsBSectionTag(prop.Name)}";
        //	//	remoteResult.Text += "\n";
        //	//}
        //	//await Client.EditMessageteTextAsync(remoteResult.Chat.Id, remoteResult.MessageId, remoteResult.Text, ParseMode.Html);
        //	//.Parse(jsonData);
        //	//from i in jItems.Children<JObject>() 
        //	//select $"{i.Properties().Aggregate((j1, j2) => j1.Value<string>().AsBSectionTag(j1.Name))
        //	//}";
        //	//foreach (var item in jItems.Children<JObject>())
        //	//	item.Properties().ToList().ForEach(f=> f.Value. f.Name)

        //	//JObject o = new JObject();
        //	//o[nameof(InfoCommand)] = jItems;


        //	//var info = JsonConvert.DeserializeObject<Message<List<DataBaseUnit>>>(jsonData);
        //	//var strs = info.Select(s => s.ToString().AsBSectionTag(s.Name));



        //	//await Client.SendTextMessageAsync<Message<DataBaseUnit>>(
        //	//   Current.ChatId,
        //	//   o.ToString(), ParseMode.Html//,
        //	//							 //replyToMessageId: msg.MessageId,
        //	//							 //replyMarkup: new InlineKeyboardMarkup(
        //	//							 //    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
        //	//							 //)
        //	//  );
        //} 
        #endregion


        public override async Task StartHandleAsync(Info data, IHandleContext context)
        {
            var cmd = Config.GetSection(nameof(BotCommand)).GetChildren().Where(child
                => child.GetSection(CFG_KEY_COMMAND).Value == nameof(Info)).
                   Select(cmd => new MissCommand<Info>()
                   {
                       Command = cmd.GetRequiredSection(CFG_KEY_COMMAND).Value,
                       Description = cmd.GetRequiredSection(CFG_KEY_DESCRIPTION).Value,
                       CustomParams = cmd.GetRequiredSection(CFG_KEY_PARAMS).Value
                   }).FirstOrDefault();


            CurrentRequest = new SqlRawQuery(cmd.CustomParams);
            connectionString = Config.GetConnectionString("Default");
            using (var src = new CancellationTokenSource())
            {
                var result = await mm.Send(CurrentRequest, src.Token);
            };
        }
        public override Task HandleCommandAsync(Info command, string[] args)
            => throw new NotImplementedException();
        public override bool ItCanBeHandled(IHandleContext context)
            => base.ItCanBeHandled(context) && context is Update upd && upd.Message.Text == nameof(Info);
        public override Info GetDataForHandle()
        {
            var cmdInfo = ActivatorUtilities.GetServiceOrCreateInstance<Info>(Context.BotServices);
           // cmdInfo.Command = Command;
            return cmdInfo;
        }
    }
}

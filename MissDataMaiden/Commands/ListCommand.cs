using MissBot.Abstractions;
using MissBot.Handlers;

namespace MissDataMaiden
{
    public class List : IBotCommandData
    {
        public string Payload { get; set; }
        public string[] Params { get; set; }
        public string Name { get; set; }
    }

    public class ListCommandHadler : BotCommandHandler<List>
    {
        public string Payload { get; set; }       
        public string[] Params { get; set; }
        public string Name { get; set; }


        //public override async Task HandleCommandAsync(List command,string[] args)
        //{
        //	await Context.Client.WriteAsync(
        //		//Current.ChatId,
        //		nameof(StartCommand)//,
        //							//ParseMode.Markdown,
        //							//replyToMessageId: msg.MessageId,
        //							//replyMarkup: new InlineKeyboardMarkup(
        //							//    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
        //							//)
        //	);

        //}
        //public override BotCommand<List> GetDataForHandle()
        //    => new BotCommand<List>() { Command = Context.Data.Get<Message>().Command };

        public override Task HandleAsync(IContext<List> context, List data)
        {
            throw new NotImplementedException();
        }
    }
}

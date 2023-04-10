using MissBot.Abstractions;
using MissCore.Entities;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    public record List : BotCommand<ListUnit>, IBotCommand
    {

    }
    public record ListUnit : List.Unit
    {
    }
    public class ListCommandHadler : BotCommandHandler<List>
    {
        public string Payload { get; set; }
        public string[] Params { get; set; }
        public string Name { get; set; }
        public override List Command { get; }

        public override Task RunAsync(List command, IContext<List> context)
        {
            throw new NotImplementedException();
        }


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



    }
}

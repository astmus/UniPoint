using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissCore.Bot;

namespace MissDataMaiden
{
    public record List : BotUnitCommand
    {

    }


    public class ListCommandHadler : BotCommandHandler<List>
    {
        public ListCommandHadler(IRepository<BotCommand> repository)
            => this.repository = repository;

        
        private readonly IRepository<BotCommand> repository;

        public override Task HandleCommandAsync(List command,  CancellationToken cancel = default)
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

using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Entities;
using MissDataMaiden.Commands;
using MissDataMaiden.Queries;

namespace MissDataMaiden
{
    public record List : BotCommand<ListUnit>, IBotCommand
    {
        public override string Command => nameof(List);
    }
    public record ListUnit : BotUnion
    {
    }

    public class ListCommandHadler : BotCommandHandler<List>
    {
        public ListCommandHadler(IRepository<BotAction> repository)
        => this.repository = repository;
        
        static List list;
        private readonly IRepository<BotAction> repository;

      

        public override Task HandleCommandAsync(List command, IContext<List> context)
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

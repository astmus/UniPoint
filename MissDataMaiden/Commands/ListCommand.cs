using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissCore.Entities;
using MissDataMaiden.Commands;
using MissDataMaiden.Queries;
using Telegram.Bot.Types;

namespace MissDataMaiden
{
    public class List : BotCommand<ListUnit>, IBotCommand
    {
        public override string EntityAction => nameof(List);
    }
    public record ListUnit : BotUnion
    {
    }

    public class ListCommandHadler : BotCommandHandler<List>
    {
        public ListCommandHadler(IRepository<BotCommand> repository)
        => this.repository = repository;
        
        static List list;
        private readonly IRepository<BotCommand> repository;

      

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

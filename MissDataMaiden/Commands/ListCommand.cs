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

        public override List Command
        {
            get
            {
                if (list is List cmd)
                    return cmd;

                SqlQuery<Disk>.Sample.Command ??= nameof(Disk);
                return list = repository.GetAll<List>().FirstOrDefault(SqlQuery<List>.Sample);
            }
        }

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

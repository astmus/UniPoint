using MissBot.Common;
using MissBot.Handlers;
using MissBot.Interfaces;
using MissCore.Abstractions;
using Telegram.Bot.Types;

namespace MissDataMaiden.Commands
{
    public class List : BotCommand, IBotCommandData
    {
        public MissCommand<BotCommand> CommandData { get; set; }
        public string Payload { get; init; }
        public string[] Params { get; init; }
    }

    public class ListCommandHnadler : BotCommandHandler<List>, IBotCommandData
    {
        public string Payload { get; init; }
        public override bool ItCanBeHandled(IHandleContext context)
            => base.ItCanBeHandled(context) && context.Data.Get<Message>().Text == nameof(List);
       
        public string[] Params { get; init; }
        public override Task HandleCommandAsync(List command, string[] args)
        {
            Console.WriteLine($"Unhandled command {command}");
            return Task.CompletedTask;
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
        public override Task StartHandleAsync(List data, IHandleContext context)
            => throw new NotImplementedException();

        public override List GetDataForHandle()
        {
            throw new NotImplementedException();
        }
    }
}

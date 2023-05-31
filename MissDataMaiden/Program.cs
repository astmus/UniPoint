using BotService;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using MissDataMaiden.DataAccess;
using MissDataMaiden.Queries;

namespace MissDataMaiden
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var botHost = BotHost.CreateDefault(args);
            botHost.AddBot<MissDataMaid, MissDataMaidConfigurator>()
                    .Use<ExceptionHandler>()
                    .UseCommandsRepository<BotCommandsRepository>()
                    .UseCommandDispatcher<MissDataCommandDispatcher>()
                    .UseInlineAnswerHandler<MissDataAnswerHandler>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<SearchDataBaseHandler>()
                    .UseMessageHandler<MessageHandler>()
                    .AddRepository<IJsonRepository, JsonRepository>()                    
                    .AddInputParametersHandler()
                    .AddCustomCommandCreator<AddCommandHadler>()     
                    
                    .AddCommand<Disk, DiskCommandHandler>()
                    //.AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>();

            botHost.Start();
        }
    }
}

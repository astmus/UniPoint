using BotService;

namespace MissDataMaiden
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            BotHost.CreateDefault(new BotStartupConfig(), args);            
        }
    }
}

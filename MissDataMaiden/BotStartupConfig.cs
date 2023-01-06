using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotService.Configuration;
using BotService.Interfaces;

namespace MissDataMaiden
{
    public class BotStartupConfig : IBotStartupConfig
    {    

        public void ConfigureBot(IBotOptionsBuilder botBuilder)
        {
            
        }

        public void ConfigureHost(IBotConnectionOptions botConnection, IConfiguration configurationBuilder)
        {
          
        }
    }
}

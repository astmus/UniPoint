using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;

namespace BotService
{
    public abstract class BaseServiceBot : BaseBot, IBot
    {
        public abstract class Hosted<TBot> : BackgroundService where TBot : BaseBot
        {
        };
        
    }
}

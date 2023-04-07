using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Commands
{
    public class ChatRequest :  MediatR.IRequest<bool>
    {
        public IRequest<bool> Request { get; set; }        
    }
}

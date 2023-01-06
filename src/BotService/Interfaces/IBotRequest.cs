using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;

namespace BotService.Interfaces
{
    public interface IBotRequest : IRequest
    {
    }
    public interface IBotRequest<TResponse> : IBotRequest
    {
    }
}

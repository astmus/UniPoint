using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions.DataAccess;
using Telegram.Bot.Requests.Abstractions;

namespace MissBot.DataAccess.Interfacet
{
    public interface IBotContextHandler
    {
        Task<int> HandleRequestCommandAsync(string rawSQL, CancellationToken cancel = default);
    }

    public interface IRequestHandler
    {
        Task HandleRequestAsync(IRequest request, CancellationToken cancel = default);
    }

}

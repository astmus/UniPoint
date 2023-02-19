using Microsoft.Extensions.Configuration;

/* Unmerged change from project 'MissBot.Abstractions (netstandart2.0)'
Before:
using Microsoft.Extensions.DependencyInjection;

/* Unmerged change from project 'MissBot.Abstractions (netstandart2.0)'
After:
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;

/* Unmerged change from project 'MissBot.Abstractions (netstandart2.0)'
*/
using Microsoft.Extensions.DependencyInjection;

/* Unmerged change from project 'MissBot.Abstractions (netstandart2.0)'
Before:
using MissCore.Configuration;
After:
using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Configuration;
*/
using MissCore.Configuration;
using Telegram.Bot.Types;

namespace MissBot.Abstractions
{
    public interface IBot
    {
        User BotInfo { get; set; }
        void ConfigureOptions(IBotOptionsBuilder botBuilder);
        void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        Func<Update, string> ScopePredicate { get; }    
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}

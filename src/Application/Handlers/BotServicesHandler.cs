using System.Reflection;
using MissBot.Abstractions;
using MissBot.Attributes;
using MissCore.Configuration;
using MissCore.Entities;

namespace MissBot.Handlers
{
    public abstract class BotServicesHandler<TBot> : IAsyncHandler where TBot : IBot
    {
        public ExecuteHandler ExecuteHandler { get; }
        public AsyncHandler AsyncHandler { get; }

        public async Task ExecuteAsync(IHandleContext context, AsyncHandler next)
        {
            var content = (context.Get<Update<TBot>>()).Message.Text;
            var cmds = typeof(TBot).GetCustomAttributes<HasBotCommandAttribute>();
            var command = cmds.FirstOrDefault(f=> content.IndexOf(f.Name.ToLower()) > 0);
            await ExecuteCommand(Activator.CreateInstance(command.CmdType) as IBotCommandData).ConfigureAwait(false);
        }
        public abstract Task ExecuteCommand<TCommand>(TCommand command) where TCommand : IBotCommandData;
    }
}

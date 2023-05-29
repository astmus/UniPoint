using System;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissCore.Bot;

namespace MissCore.Handlers
{
    public class InputParametersHandler : BotUnitActionHadlerBase<UnitBase>
    {
        protected override void Initialize(IEnumerable<string> parameterNames)
        {
            var args = Enumerable.Repeat<AsyncInputHandler>(SetParameter, currentUnit.ArgumentCount).ToArray();
            JoinHandlers(args);            
        }

        async Task Save(IHandleContext context)
        {
            var listCmds = context.Bot.Commands.ToList();
            //listCmds.Add(add);
            var scope = Telegram.Bot.Types.BotCommandScope.Chat(context.Get<Chat>().Id);
            var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, scope);
            if (success)
            {
             //   context.Bot.Commands.Add(add);
                Response.CompleteInput("Command added");
            }
            else
                Response.CompleteInput("Save failed");

            context.IsHandled = true;
        }

        Task Cancel(IHandleContext context)
        {
            Response.CompleteInput("Operation cancelled");
            context.IsHandled = true;
            return Task.CompletedTask;
        }

        object SetParameter(IHandleContext context, string input, string parameterName) => input switch
        { 
            null when currentUnit[parameterName] is null =>
                Response.InputData($"Enter parameter: '{parameterName}'"),            
            var value when value.IndexOf(' ') < 0 =>
                currentUnit[parameterName] = value,
            var value when value.IndexOf(' ') > -1 =>
                ReTry(SetParameter, "Invalid parameter format"),
            _ => null
        };  
    }
}

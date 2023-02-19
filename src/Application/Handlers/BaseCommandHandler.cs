using System.Collections.Concurrent;
using System.Reflection;using Microsoft.Extensions.DependencyInjection;using MissBot.Abstractions;
using MissBot.Attributes;using MissBot.Extensions.Entities;using MissCore.Abstractions;using MissCore.Entities;namespace MissBot.Handlers{    public class BaseCommandHandler<TBot> : IAsyncHandler where TBot : IBot    {
        static ConcurrentDictionary<string, (Type,Type)> commands;
        public virtual Task BeforeComamandHandle(IHandleContext context)
            => Task.CompletedTask;
        public virtual Task AfterComamandHandle(IHandleContext context)
            => Task.CompletedTask;
        public virtual Task OnComamandFailed(IHandleContext context, Exception error)
            => Task.CompletedTask;        public virtual async Task ExecuteAsync(IHandleContext context, HandleDelegate next)        {            commands ??= new ConcurrentDictionary<string, (Type, Type)>(typeof(TBot).GetCustomAttributes<HasBotCommandAttribute>().Select(s
                =>new KeyValuePair<string, (Type, Type)>($"/{s.Name.ToLower()}", (s.CmdType, typeof(IAsyncHandler<>).MakeGenericType(s.CmdType)))));
            try
            {
                if (context.Update is Update<TBot> botUpdate)
                {
                    var cmdData = botUpdate.Message?.GetCommandAndArgs();

                    (Type commandType, Type handler) handlePair;
                    if (cmdData.HasValue && commands.TryGetValue(cmdData.Value.command, out handlePair))
                    {
                        await BeforeComamandHandle(context).ConfigureAwait(false);
                        var command = ActivatorUtilities.GetServiceOrCreateInstance(context.BotServices, handlePair.commandType) as IBotCommandData;
                        command.Params = cmdData.Value.args;

                        if (context.BotServices.GetService(handlePair.handler) is IAsyncBotCommansHandler commandHandler)
                            await commandHandler.HandleCommandAsync(context, command).ConfigureAwait(false);

                        //var result = from info in handlePair.commandType.GetProperties().Where(p => p.GetCustomAttributes<BotCommandResultAttribute>() != null).ToList()
                        //             select info.GetValue(command);
                        //context.ContextData.Set(result.ToList(), nameof(result));
                        await AfterComamandHandle(context).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                await OnComamandFailed(context, ex).ConfigureAwait(false);
            }
            finally {
                await next(context).ConfigureAwait(false);
            }        }    }}
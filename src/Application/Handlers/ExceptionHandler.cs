using MissBot.Abstractions;
using MissCore.Handlers;

namespace MissBot.Handlers
{
    public class ExceptionHandler : BaseHandleComponent
    {
        protected override Task HandleAsync(IHandleContext context)
        {
            return ExecuteAsync(context);
        }
        public override async Task ExecuteAsync(IHandleContext context)
        {
            try
            {                
                await context.GetNextHandler().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await context.BotServices.ResponseError().Write(e).Commit();
                context.IsHandled = true;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured in handling update {0}.{1}{2}", context.CurrentHandler, Environment.NewLine, e);
                Console.ResetColor();
            }
        }
    }
}

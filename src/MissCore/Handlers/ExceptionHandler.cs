using MissBot.Abstractions;

namespace MissCore.Handlers
{
    public class ExceptionHandler : BaseHandleComponent
    {
        public async override Task HandleAsync(IHandleContext context, AsyncHandler next, CancellationToken cancel = default)
        {
            try
            {
                await context.GetNextHandler(next).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await context.BotServices.ResponseError().Write(e).Commit(cancel);
                context.IsHandled = true;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured in handling update {0}.{1}{2}", context.CurrentHandler, Environment.NewLine, e);
                Console.ResetColor();
            }

        }
    }
}

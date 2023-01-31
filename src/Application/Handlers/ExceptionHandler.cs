using MissCore.Abstractions;
using MissCore.Handlers;

namespace MissBot.Handlers
{
    public class ExceptionHandler : IAsyncHandler
    {
        public async Task ExecuteAsync(IHandleContext context, HandleDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured in handling update {0}.{1}{2}", context.Update, Environment.NewLine, e);
                Console.ResetColor();
            }
        }
    }
}

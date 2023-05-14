using System.Runtime.CompilerServices;
using AsyncAwaitBestPractices;


namespace BotService
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable ConfigFalse(this Task task)
            => task.ConfigureAwait(false);
        public static void SafeFire(this Task task)
            => task.SafeFireAndForget();
        public static ConfiguredValueTaskAwaitable ConfigFalse(this ValueTask task)
            => task.ConfigureAwait(false);
        public static ConfiguredValueTaskAwaitable<TResult> ConfigFalse<TResult>(this ValueTask<TResult> task)
            => task.ConfigureAwait(false);
        public static ConfiguredTaskAwaitable<TResult> ConfigFalse<TResult>(this Task<TResult> task)
            => task.ConfigureAwait(false);

    }
}

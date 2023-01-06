using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot.Types;

namespace BotService.Common
{

    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string EncodeUtf8(this string value) =>
            new(Encoding.UTF8.GetBytes(value).Select(c => Convert.ToChar(c)).ToArray());

        internal static (string command, string[] args) GetCommandAndArgs(this Message message)
            => ParseCommand(message.Text);
        internal static (string command, string[] args) GetCommandAndArgs(this CallbackQuery query)
            => ParseCommand(query.Data);
        static (string command, string[] args) ParseCommand(string message)
        {
            if (message.Contains(" "))
            {
                var items = message.Split("--");
                return (items[0], items[1..]);
                //return (items[0], items[1..^1]);
            }
            else
                return (message, null);
        }
        //public static string[] GetArgs(this InlineQuery query)
        //    => query.Query.Contains("--") ? query.Query.Split("--") : new string[] { query.Query };
    }
}

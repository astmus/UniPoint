using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot.Types;

namespace MissBot.Extensions
{

    public static class StringExtensions
    {        
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

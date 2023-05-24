using MissBot.Entities;
using MissBot.Entities.Query;

namespace MissBot.Extensions.Entities
{
    public static class CommandExtensions
    {

        public static (string command, string[] args) GetCommandAndArgs(this Message message)
            => ParseCommand(message.Text);
        public static string Capitalize(this string text)
            => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        public static string Capitalize(this ReadOnlySpan<char> text)
            => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToString());
        public static (string command, string[] args) GetCommandAndArgs(this CallbackQuery query)
            => ParseCommand(query.Data);
        static (string command, string[] args) ParseCommand(string message)
        {
            if (message.Contains("/"))
            {
                var items = message.Split("/", StringSplitOptions.RemoveEmptyEntries);
                return (items[0].AsSpan(0).Capitalize(), items[1..]);
            }
            return (null, null);
        }
        //public static string[] GetArgs(this InlineQuery query)
        //    => query.Query.Contains("--") ? query.Query.Split("--") : new string[] { query.Query };
    }
}

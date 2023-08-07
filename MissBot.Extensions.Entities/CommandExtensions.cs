using MissBot.Entities;
using MissBot.Entities.Query;
using System.Globalization;

namespace MissBot.Extensions
{
	public static class CommandExtensions
	{
		public static (string command, ICollection<string> args) GetCommandAndArgs(this Message message)
			=> ParseCommand(message.Text);

		public static string Capitalize(this string text)
			=> CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);

		public static string Capitalize(this ReadOnlySpan<char> text)
		{
			var cU = CultureInfo.CurrentCulture.TextInfo.ToUpper(text[0]);// .ToTitleCase(text.ToString());
			return $"{cU}{text[1..]}";
		}

		public static (string command, ICollection<string> args) GetCommandAndArgs(this CallbackQuery query)
			=> ParseCommand(query.Data);

		static (string command, ICollection<string> args) ParseCommand(string message)
		{
			var iterator = message.SplitCommandArguments();

			if (iterator.MoveNext())
			{
				var res = (command: iterator.Current.Segment.Capitalize(), args: new List<string>());
				while (iterator.MoveNext())
					res.args.Add(iterator.Current);
				return res;
			}

			return default;
		}
	}
}

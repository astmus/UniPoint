using MissBot.Entities.Query;
using MissBot.Extensions;
namespace MissCore.Extensions
{

	public static class StringExtensions
	{

		internal static (string unit, string command, string[] args) GetCommandArguments(this CallbackQuery query)
			=> ParseCommand(query.Data);
		static (string unit, string command, string[] args) ParseCommand(string message)
		{
			if (message.Contains("."))
			{
				var iterator = MissBot.Extensions.StringExtensions.SplitBy(message, '.').GetEnumerator();
				var itemsCount = iterator.SlicesCount();
				string[] items = new string[itemsCount];
				byte index = 0;

				while (iterator.MoveNext())
					items[index++] = iterator.Current;

				return (items[0], items[1], items[2..]);
			}
			else
				return (message, null, null);
		}
		//public static string[] GetArgs(this InlineQuery query)
		//    => query.Query.Contains("--") ? query.Query.Split("--") : new string[] { query.Query };
	}
}

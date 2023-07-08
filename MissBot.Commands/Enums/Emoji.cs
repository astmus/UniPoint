using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Emoji on which the dice throw animation is based
/// <remarks>
/// This enum is used only in the library APIs and is not present in types that are coming from
/// Telegram servers for compatibility reasons
/// </remarks>
/// </summary>
[JsonConverter(typeof(EmojiConverter))]
public enum Emoji
{
	/// <summary>
	/// Dice. Resulting value is 1-6
	/// </summary>
	[Display(Name = "🎲")]
	Dice = 1,

	/// <summary>
	/// Darts. Resulting value is 1-6
	/// </summary>
	[Display(Name = "🎯")]
	Darts,

	/// <summary>
	/// Basketball. Resulting value is 1-5
	/// </summary>
	[Display(Name = "🏀")]
	Basketball,

	/// <summary>
	/// Football. Resulting value is 1-5
	/// </summary>
	[Display(Name = "⚽")]
	Football,

	/// <summary>
	/// Slot machine. Resulting value is 1-64
	/// </summary>
	[Display(Name = "🎰")]
	SlotMachine,

	/// <summary>
	/// Bowling. Result value is 1-6
	/// </summary>
	[Display(Name = "🎳")]
	Bowling
}


internal partial class EmojiConverter : JsonConverter<Emoji>
{
	public override void WriteJson(JsonWriter writer, Emoji value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			Emoji.Dice => "🎲",
			Emoji.Darts => "🎯",
			Emoji.Basketball => "🏀",
			Emoji.Football => "⚽",
			Emoji.SlotMachine => "🎰",
			Emoji.Bowling => "🎳",
			(Emoji)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override Emoji ReadJson(
		JsonReader reader,
		Type objectType,
	Emoji existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"🎲" => Emoji.Dice,
			"🎯" => Emoji.Darts,
			"🏀" => Emoji.Basketball,
			"⚽" => Emoji.Football,
			"🎰" => Emoji.SlotMachine,
			"🎳" => Emoji.Bowling,
			_ => 0,
		};
}

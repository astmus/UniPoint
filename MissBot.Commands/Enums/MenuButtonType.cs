using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Type of the <see cref="MenuButton"/>
/// </summary>
[JsonConverter(typeof(MenuButtonTypeConverter))]
public enum MenuButtonType
{
	/// <summary>
	/// Describes that no specific value for the menu button was set.
	/// </summary>
	Default = 1,

	/// <summary>
	/// Represents a menu button, which opens the bot’s list of commands.
	/// </summary>
	Commands,

	/// <summary>
	/// Represents a menu button, which launches a <a href="https://core.telegram.org/bots/webapps">Web App</a>.
	/// </summary>
	WebApp
}


internal partial class MenuButtonTypeConverter : JsonConverter<MenuButtonType>
{
	public override void WriteJson(JsonWriter writer, MenuButtonType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			MenuButtonType.Default => "default",
			MenuButtonType.Commands => "commands",
			MenuButtonType.WebApp => "web_app",
			(MenuButtonType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override MenuButtonType ReadJson(
		JsonReader reader,
		Type objectType,
	MenuButtonType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"default" => MenuButtonType.Default,
			"commands" => MenuButtonType.Commands,
			"web_app" => MenuButtonType.WebApp,
			_ => 0,
		};
}

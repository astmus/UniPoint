using MissBot.Entities.Query;
using Telegram.Bot.Types;

namespace MissBot.Entities.Results;

/// <summary>
/// This object represents a result of an <see cref="InlineQuery"/> that was chosen by the <see cref="User"/>
/// and sent to their chat partner.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class ChosenInlineResult
{
	public ChosenInlineResult()
	{

	}

	void UpdateValues()
	{
		try
		{
			var strSpan = ResultId.AsSpan();
			var unitundex = strSpan.IndexOf('.');
			var queryindex = strSpan.LastIndexOf('.');
			Unit = strSpan[0..unitundex].ToString();
			unitundex++;
			Id = strSpan[unitundex..queryindex].ToString();
			queryindex++;
			Query = strSpan[queryindex..].ToString();
		}
		catch
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine(nameof(ChosenInlineResult) + " can't parse result ID");
			Console.ResetColor();
		}
	}

	public string Id { get; protected set; }
	public string Unit { get; protected set; }

	/// <summary>
	/// The unique identifier for the result that was chosen.
	/// </summary>
	[JsonProperty(Required = Required.Always)]
	public string ResultId
	{
		get => _resultId;
		set
		{
			_resultId = value;
			UpdateValues();
		}
	}
	string _resultId;
	/// <summary>
	/// The user that chose the result.
	/// </summary>
	[JsonProperty(Required = Required.Always)]
	public User From { get; set; } = default!;

	/// <summary>
	/// Optional. Sender location, only for bots that require user location
	/// </summary>
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public Location Location { get; set; }

	/// <summary>
	/// Optional. Identifier of the sent inline message. Available only if there is an inline keyboard attached
	/// to the message. Will be also received in callback queries and can be used to edit the message.
	/// </summary>
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string InlineMessageId { get; set; }

	/// <summary>
	/// The query that was used to obtain the result.
	/// </summary>
	[JsonProperty(Required = Required.Always)]
	public string Query { get; set; } = default!;
}

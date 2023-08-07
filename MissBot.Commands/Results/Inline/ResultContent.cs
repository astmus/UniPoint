using MissBot.Entities;
using MissBot.Entities.Enums;
using MissBot.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissBot.Entities.Results.Inline
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record ResultContent
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public bool? DisableWebPagePreview { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public MessageEntity[] Entities { get; set; }

		[JsonProperty("message_text")]
		public string Content { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public ParseMode? ParseMode { get; set; } = Enums.ParseMode.Html;
	}


	public record ResultContent<TUnit> : ResultContent
	{
		public virtual object Identifier
			=> Id<TUnit>.Instance;
	}
}

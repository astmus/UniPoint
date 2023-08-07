using System.Runtime.Serialization;

using LinqToDB.Mapping;

using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using MissCore.Bot;
using MissCore.Data;

namespace MissCore.Actions;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
//[Table("##UnitActions")]
public record UnitAction<TEntity> : UnitAction, IUnitAction<TEntity> //where TEntity : class
{
	public virtual Id Id { get; set; } = Id<TEntity>.Instance;
	public UnitAction()
	{
	}

	public UnitAction(string text) : base(text)
	{
	}

	public virtual UnitAction<TEntity> WithUnitContextId(IIdentibleUnit unit)
	{
		Identifier = CallbackData = $"{Unit}.{Action}.{unit.Identifier}";
		return this;
	}

	IMetaData Meta;
	public void SetUnitContext<TCUnit>(TCUnit unit) where TCUnit : class, TEntity
	{
		Meta = Data.Unit.MetaData.FromObject<MetaData<TCUnit>>(unit);
	}

	[JsonProperty("text")]
	public override string Action { get; set; }

	public virtual string Format { get; set; }
}

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
//[Table("##UnitActions")]
//[Column(nameof(Unit), nameof(Action))]
public record UnitAction : BotAction, IUnitAction, IParameterizedUnit, ISerializable
{
	//[Column]
	//public override string Action { get; set; }

	//public override string Entity => base.Entity;

	//[Column("Unit", IsDiscriminator = true)]
	//[Column]
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public override string Unit { get; set; }

	//[Column]
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public override string Template { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string? Url { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string? SwitchInlineQuery { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string? SwitchInlineQueryCurrentChat { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public SwitchInlineQueryChosenChat? SwitchInlineQueryChosenChat { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool? Pay { get; set; }

	[JsonIgnore]
	public virtual IIdentibleUnit UnitContext { get => _unit; set { _unit = value; CallbackData = Identifier.ToString(); } }
	IIdentibleUnit _unit;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string CallbackData { get; set; }

	public override object Identifier { get; set; }
	//=> $"{Unit}.{Action}.{UnitContext.Identifier}";

	[JsonConstructor]
	public UnitAction(string text) : this()
		=> Action = text;

	public UnitAction()
	{
		//CallbackData = $"{Action}.{Unit}.";
	}
	//public static UnitAction WithUrl(string text, string url) =>
	//    new(text) { Url = url };
	//[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	//public WebAppInfo? WebApp { get; set; }

	//[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	//public LoginUrl? LoginUrl { get; set; }

	//[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	//public CallbackGame? CallbackGame { get; set; }
	//[Column]
	//[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	//public override string Parameters { get; }


	//public static InlineUnitAction WithLoginUrl(string text, LoginUrl loginUrl) =>
	//    new(text) { LoginUrl = loginUrl };

	//public static UnitAction WithSwitchInlineQuery(string text, string query = "") =>
	//    new(text) { SwitchInlineQuery = query };

	//public static UnitAction WithSwitchInlineQueryCurrentChat(string text, string query = "") =>
	//    new(text) { SwitchInlineQueryCurrentChat = query };

	//public static UnitAction WithSwitchInlineQueryChosenChat(string text, SwitchInlineQueryChosenChat switchInlineQueryChosenChat) =>
	//    new(text) { SwitchInlineQueryChosenChat = switchInlineQueryChosenChat };

	//public static InlineUnitAction WithCallBackGame(string text, CallbackGame? callbackGame = default) =>
	//    new(text) { CallbackGame = callbackGame ?? new() };

	//public static UnitAction WithPayment(string text) =>
	//    new(text) { Pay = true };

	//public virtual IUnitAction<IIdentible> SetUnit<TUnit>(TUnit unit) where TUnit : class, IIdentible, IUnitEntity
	//    => new UnitAction<TUnit>(Action) { Entity = unit };

	//public static InlineUnitAction WithWebApp(string text, WebAppInfo webAppInfo) =>
	//    new(text) { WebApp = webAppInfo };

	public static implicit operator UnitAction?(string? textAndCallbackData) =>
		textAndCallbackData is null
			? default
			: new UnitAction(textAndCallbackData);

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		int i = 0;
	}
}

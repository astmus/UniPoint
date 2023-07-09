using System.Text.Json.Nodes;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Utils;
using MissBot.Identity;
using MissCore.Data;
using MissCore.Data.Collections;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public record ContentUnit<TData> : Unit, IContentUnit<TData>, IUnitContainable<TData> where TData : class
	{
		[JsonProperty(nameof(Content))]
		Union<TData> _content;
		Position _posIndex;
		protected Union<TData> content
		{
			get =>
				_content ?? (_content = new Union<TData>(Enumerable.Empty<JToken>()));
			set =>
				_content = value;
		}

		public override string Unit
			=> Id<TData>.Instance.Key;

		[JsonIgnore]
		public IUnitCollection<TData> Content
			=> _content;

		public void Add<TUnit>(TUnit unit) where TUnit : IUnit<TData>
			=> content.Add(unit);

		public void Add(IUnit<TData> unit)
		{
			content.Add(unit);
		}

		public MetaType ContentType => _content switch
		{
			null => MetaType.Null,
			{ Count: 0 } => MetaType.Empty,
			_ => MetaType.Union
		};

		IMetaCollection IContentUnit.Content { get; }

		public record Empty : Unit<TData>
		{
			public override string Unit
				=> Unit<TData>.Key;
		}
	}
}

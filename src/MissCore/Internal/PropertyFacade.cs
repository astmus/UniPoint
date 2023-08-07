using Microsoft.Extensions.DependencyInjection;

using MissBot.Abstractions;
using MissBot.Entities.Abstractions;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MissCore.Internal
{
	internal record PropertyFacade : IUnitItem
	{
		public static readonly PropertyFacade Instance = new PropertyFacade();

		private readonly IServiceProvider provider;
		internal protected JProperty context { get; set; }
		public string Name
			=> this switch
			{
				{ context.Parent: { } } _this when context.Parent is JProperty property => property.Name,
				_ => context.Name
			};

		public object Value
			=> context.Value switch
			{
				JValue value => value.Value,
				JProperty prop => prop.Value,
				JArray array => null,
				_ => throw new Exception($"value is no J is {context.Value}")
			};

		public IUnitItem Wrap(JProperty prop)
		{
			context = prop;
			return this;
		}

		public string Serialize()
			=> $"{Name}: {Value}";
	}
}

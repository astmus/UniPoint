using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MissCore.Internal
{
	internal class PropertyFacade : IUnitItem
	{
		private readonly IServiceProvider provider;
		protected JProperty context { get; set; }
		public string ItemName
			=> this switch
			{
				{ context.Parent: { } } _this when context.Parent is JProperty property => property.Name,
				_ => context.Name
			};

		public object ItemValue
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
			=> $"{ItemName}: {ItemValue}";
	}
}

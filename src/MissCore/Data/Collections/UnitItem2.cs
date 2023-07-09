using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{
	//public readonly record struct MetaItem(string name, string value) : IMetaItem
	//{
	//    public string Format(IMetaItem.Formats format)
	//    {
	//        uint bvalue = 0x01;
	//        uint bits = Convert.ToUInt32(format);
	//        var strFormat = String.Empty;
	//        while (bvalue <= bits)
	//        {
	//            var b = bvalue & bits;
	//            if ((bvalue & bits) > 0)
	//                strFormat += GetFormat((IMetaItem.Formats)b);
	//            bvalue <<= 1;
	//        }
	//        return string.Format(strFormat, name, value);
	//    }
	//    static string GetFormat(IMetaItem.Formats format) => format switch
	//    {
	//        IMetaItem.Formats.NewLine => "\n",
	//        IMetaItem.Formats.B => "<b>{0} {1}</b>",
	//        IMetaItem.Formats.I => "<i>{0} {1}</i>",
	//        IMetaItem.Formats.Code => "<code>{0} {1}</code>",
	//        IMetaItem.Formats.Strike => "<s>{0} {1}</s>",
	//        IMetaItem.Formats.Under => "<u>{0} {1}</u>",
	//        IMetaItem.Formats.Pre => "<pre>{0} {1}</pre>",
	//        IMetaItem.Formats.Link => "<a href=\"{0}\">{1}</a>",
	//        IMetaItem.Formats.BSection => "<b>{0}</b>: {1}",
	//        IMetaItem.Formats.Percent => " % ",
	//        IMetaItem.Formats.Equal => " = ",
	//        IMetaItem.Formats.Section => "{0}: {1}",
	//        _ => String.Empty
	//    };
	//    public override string ToString()
	//    {
	//        return ToString(default, default);
	//    }
	//    public string ToString(string format, IFormatProvider formatProvider)
	//        => $"{name}: {value}";
	//}
	// Provides the Create factory method for KeyValuePair<TKey, TValue>.
	public class UnitItem : JProperty, IUnitItem
	{
		public UnitItem(JProperty other) : base(other)
		{
		}

		public string ItemName
			=> this switch
			{
				{ Parent: { } } _this when _this.Parent is JProperty property => property.Name,
				_ => Name
			};

		public object ItemValue
			=> Value switch
			{
				JValue value => value.Value,
				JProperty prop => prop.Value,
				JArray array => null,
				_ => throw new Exception($"value is no J is {Value}")
			};

		public override string ToString()
			=> Serialize();

		public virtual string Serialize()
			=> $"{ItemName}: {ItemValue}";
	}

	//public readonly record struct UnitItem23(JProperty token) : IUnitItem
	//{
	//    public string ItemName
	//        => token switch
	//        {
	//            { Parent: { } } v when v.Parent is JProperty pa => pa.Name,
	//            _ => token.Name
	//        };

	//    public object ItemValue
	//        => token.Value is JValue val ? val.Value : null;
	//    public override string ToString()
	//        => Serialize();
	//    public string Serialize()
	//        => $"{ItemName}: {ItemValue}";
	//}
}

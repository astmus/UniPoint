using MissBot.Abstractions.Utils;

namespace MissBot.Abstractions
{
    public interface IUnitRequest
    {
        RequestOptions RequestOptions { get; set; }
        string GetCommand(RequestOptions options = RequestOptions.JsonAuto);
    }

    public interface IUnitQuery : IFormattable
    {
        IEnumerable<TResult> ExecuteAsync<TResult>(RequestOptions format = RequestOptions.JsonAuto);
    }

    [Flags]
    [JsonConverter(typeof(RequestOptions))]
    public enum RequestOptions
    {
        Unknown = 0,
        Raw = 1,
        JsonAuto = 2,
        JsonPath = 4,
        Scalar = 8
    }

    public static class FormatExtension
    {
        const string JSONNoWrap = ", WITHOUT_ARRAY_WRAPPER";
        public static string ApplyTo(this RequestOptions format, IUnitRequest cmd)
            => cmd.ToString() + format.TrimSnakes();

        public static string TrimSnakes(this RequestOptions format)
            => format switch
            {
                RequestOptions.Unknown => $"Unknown format {format}",
                RequestOptions.Raw => format.ToString(),
                RequestOptions.JsonAuto => $" for {format.ToString().ToSnakeCase(true)}",
                RequestOptions.JsonAuto | RequestOptions.Scalar => $" for {RequestOptions.JsonAuto.ToString().ToSnakeCase(true)} {JSONNoWrap}",
                RequestOptions.JsonPath => $" for {format.ToString().ToSnakeCase(true)}  {JSONNoWrap}",
                _ => format.ToSnakeCase()
            };
        public static string SnakeTemplate(this RequestOptions format)
                    => $"{{0}} {format.TrimSnakes()}";
        public static string Make(this CriteriaFormat format)
            => format switch
            {
                CriteriaFormat.Unknown => $"Unknown format {format}",
                CriteriaFormat.SQL => " WHERE {0} {1} {2} ",
                _ => format.ToString()
            };
    }
}

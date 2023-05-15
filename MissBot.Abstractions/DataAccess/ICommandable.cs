using MissBot.Abstractions.Utils;

namespace MissBot.Abstractions.DataAccess
{
    public interface IRepositoryCommand : IFormattable
    {
        string ToRequest(RequestFormat format = RequestFormat.JsonAuto);
    }

    [JsonConverter(typeof(RequestFormat))]
    public enum RequestFormat
    {
        Unknown,
        Raw,
        JsonAuto,
        JsonPath
    }

    public static class FormatExtension
    {
        public static string TrimSnakes(this RequestFormat format)
            => format switch
            {
                RequestFormat.Unknown => $"Unknown format {format}",
                RequestFormat.Raw => format.ToString(),
                RequestFormat.JsonAuto => $" for {format.ToString().ToSnakeCase(true)}",
                RequestFormat.JsonPath => $" for {format.ToString().ToSnakeCase(true)}",
                _ => format.ToSnakeCase()
            };

        public static string Make(this CriteriaFormat format)
            => format switch
            {
                CriteriaFormat.Unknown => $"Unknown format {format}",
                CriteriaFormat.SQL => " WHERE {0} {1} {2} ",
                _ => format.ToString()
            };
    }
}

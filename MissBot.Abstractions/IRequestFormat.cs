using MissBot.Abstractions.Utils;

namespace MissBot.Abstractions
{
    public interface IRepositoryCommand : IFormattable
    {
        string ToRequest(RequestFormat format = RequestFormat.JsonAuto);
        IRepositoryCommand SingleResult();
    }
    [Flags]
    [JsonConverter(typeof(RequestFormat))]
    public enum RequestFormat
    {
        Unknown = 0,
        Raw = 1,
        JsonAuto = 2,
        JsonPath = 4,
        SingleResult = 8
    }

    public static class FormatExtension
    {
        const string JSONNoWrap = ", WITHOUT_ARRAY_WRAPPER";
        public static string ApplyTo(this RequestFormat format, IRepositoryCommand cmd)
            => cmd.ToString(default,default) + format.TrimSnakes();
        public static string TrimSnakes(this RequestFormat format)
            => format switch
            {
                RequestFormat.Unknown => $"Unknown format {format}",
                RequestFormat.Raw => format.ToString(),
                RequestFormat.JsonAuto => $" for {format.ToString().ToSnakeCase(true)}",
                RequestFormat.JsonAuto | RequestFormat.SingleResult => $" for {format.ToString().ToSnakeCase(true)} {JSONNoWrap}",
                RequestFormat.JsonPath => $" for {format.ToString().ToSnakeCase(true)}  {JSONNoWrap}",
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

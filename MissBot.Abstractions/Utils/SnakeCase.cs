using System.Text.Json;

namespace MissBot.Abstractions.Utils
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
            => name.ToSnakeCase();
    }
    public static class JsonSerializationExtensions
    {
        private static readonly SnakeCaseNamingStrategy _snakeCaseNamingStrategy
            = new SnakeCaseNamingStrategy();

        private static readonly JsonSerializerSettings _snakeCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = _snakeCaseNamingStrategy
            }
        };

        public static string ToSnakeCase<T>(this T instance) => instance switch
        {
            null => throw new ArgumentNullException(paramName: nameof(instance)),
            _ => JsonConvert.SerializeObject(instance, _snakeCaseSettings)
        };
        public static string ToSnakeCase(this string target)
            => target.ToSnakeCase(false);
        public static string ToSnakeCase(this string target, bool trimSnakes) => (target,trimSnakes) switch
        {
            (not null, false)
                => _snakeCaseNamingStrategy.GetPropertyName(target, false),
            (not null, true)
                => _snakeCaseNamingStrategy.GetPropertyName(target, false).Replace("_", " "),            
            _   => throw new ArgumentNullException(paramName: nameof(target))
    };
    }
}

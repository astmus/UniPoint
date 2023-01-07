using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Text.RegularExpressions;

namespace BotService.Common
{
    public static class DataTransformExtension
    {
        private const char pathSeparator = '.';
        private static readonly Regex arrayItemPathRegexp = new Regex("^(.+)\\[(\\d+)\\]$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        public static readonly JsonSerializerSettings ContentSerializerSettings =
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Include,
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        //Formatting = Formatting.Indented,
                        //MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                        //TypeNameHandling = TypeNameHandling.Objects,										

                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    };


        public static string ToStringJson<T>(this T obj, JsonSerializerSettings serializerSettings = null) =>
            JsonConvert.SerializeObject(obj, serializerSettings ?? ContentSerializerSettings);

        public static T FromJsonString<T>(this string json, JsonSerializerSettings serializerSettings = null) =>
            JsonConvert.DeserializeObject<T>(json, serializerSettings ?? ContentSerializerSettings);

        public static T CopyAs<T>(this object obj)
            => CopyAs<T>(ContentSerializerSettings);

        public static T CopyAs<T>(this object obj, JsonSerializerSettings options) => obj switch
        {
            null => default,
            JObject jobj => jobj.ToObject<T>(),
            string data => JsonConvert.DeserializeObject<T>(data, options),
            _ => JsonConvert.SerializeObject(obj, options).CopyAs<T>(options)
        };
    }
}

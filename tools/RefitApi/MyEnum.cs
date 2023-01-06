using System.ComponentModel;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace RefitApi
{
    /// <summary>
    /// The type of an <see cref="Update"/>
    /// </summary>
    [JsonConverter(typeof(MyEnumConverter))]
    public enum MyEnum 
    {
        /// <summary>
        /// The <see cref="Update"/> contains a <see cref="Message"/>.
        /// </summary>
        k1,
        /// <summary>
        /// e
        /// </summary>
        k2,
        /// <summary>
        /// t
        /// </summary>
        k3,
        /// <summary>
        /// y
        /// </summary>
        k5,
        /// <summary>
        /// u
        /// </summary>
        k7
    }
}

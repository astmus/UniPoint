using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class BotCommand
    {
        /// <summary>
        /// Text of the command, 1-32 characters. Can contain only lowercase English letters, digits and underscores.
        /// </summary>
        [JsonProperty("command", Required = Required.Always)]
        public virtual string Command { get; set; }

        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FormatParticleAttribute : Attribute
    {
        public string Name { get; init; }
        public string Format { get; set; }
        public FormatParticleAttribute(string name)
            => Name = name;

        public string Apply(string objName)
        {
            if (Name != null || objName != null)
                return Format is null ? Name ?? objName : string.Format(Format, Name ?? objName);
            return null;
        }
    }
}

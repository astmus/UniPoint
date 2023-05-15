using MissCore.Collections;

namespace MissBot.Utils
{
    public class CriteriaFormatter : ICustomFormatter
    {
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return string.Format(format, arg);
        }
    }
}

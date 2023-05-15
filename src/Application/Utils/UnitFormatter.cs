using MissCore.Collections;

namespace MissBot.Utils
{
    public  class UnitFormatter : IFormatProvider, ICustomFormatter
    {
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return  string.Format(format, arg);
        }

        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(Unit))
                return this;
            else return null;
        }
    }
}

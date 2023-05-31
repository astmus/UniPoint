using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{
    public class GenericUnit : ListDictionary,IFormattable
    {
        public new IEnumerable<string> Keys
            => base.Keys.Cast<string>();
        public Unit<T> ToUnit<T>()
            => Unit<T>.Parse(this);
        public string StringValue
            => string.Join(" ", Keys.Select(key => $"{key}: {this[key]}"));
        public string Format(string? format)
            => ToString(format, default);
        public string ToString(string? format, IFormatProvider? provider)
        {
            if (String.IsNullOrEmpty(format)) format = "T";
            if (provider == null) provider = CultureInfo.CurrentCulture;
            switch (format.ToUpperInvariant())
            {
                case "T":
                    return string.Join(" ", Keys.Select(key => $"{key}: {this[key]}\n"));
                case "TB":
                    return string.Join(" ", Keys.Select(key => $"<b>{key}:</b> {this[key]}\n"));
            }
            return StringValue;
         }
    }
}

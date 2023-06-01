using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{
    public interface IGenericUnit
    {
        IEnumerable<string> ItemNames { get; }
        IEnumerable<string> ItemValues { get; }
        string StringValue { get; }

        Unit<T> ToUnit<T>();
    }

    public class GenericUnit : ListDictionary, IGenericUnit
    {
        public IEnumerable<string> ItemNames
            => Keys.Cast<string>();
        public IEnumerable<string> ItemValues
        => Values.Cast<string>();
        public Unit<T> ToUnit<T>()
            => Unit<T>.Parse(this);
        public string StringValue
            => string.Join(" ", ItemNames.Select(key => $"{key}: {this[key]}"));

    }
}

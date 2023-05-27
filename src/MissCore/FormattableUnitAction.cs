using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;

namespace MissCore
{
    public class FormattableUnitAction : FormattableUnitActionBase
    {
        private string _format;
        private readonly List<object> _arguments;
        public Position? Parameter;
        private readonly ListDictionary _parameters = new ListDictionary();
        public uint BackParameter
            => Parameter.Value.Back;
        public uint ForwardParameter
            => Parameter.Value.Forward;
        public uint ParameterIndex
            => Parameter.Value.Current;

        internal FormattableUnitAction(string format)
            => _format = format;
        public string this[uint index]
            => _parameters.Keys.Cast<string>().ElementAtOrDefault(Convert.ToInt32(index));

        internal FormattableUnitAction(string format, IEnumerable<object> args = default) : this(format)
        {
            if (args != null)
                _arguments = new List<object>(args);
        }
        internal FormattableUnitAction(string format, IEnumerable<string> parameters = default) : this(format)
        {
            if (parameters != null)
                foreach (var p in parameters)
                    _parameters.Add("@" + p, null);
        }

        internal static FormattableUnitAction Create(string format)
           => new FormattableUnitAction(format);
        internal static FormattableUnitAction Create(string format, params object[] args)
            => new FormattableUnitAction(format, args);
        internal static FormattableUnitAction Create(string format, params string[] args)
            => new FormattableUnitAction(format, args);

        public override string Format
            => _format;
        public override object[] GetArguments()
            => _arguments.ToArray();
        public override int ArgumentCount
            => _arguments?.Count ?? _parameters?.Count ?? 0;
        public string CurrentParameterName
            => this[Parameter.Value.Current];
        public override object GetArgument(int index)
            => _parameters[index];
        public object this[string key]
        {
            get => _parameters[key];
            set => _parameters[key] = value;
        }
        public void SetupParameterPosition()
        {
            if (Parameter.HasValue)
                return;

            byte pos = 0;
            foreach (DictionaryEntry de in _parameters)
                if (de.Value != null)
                    pos++;
                else
                    break;

            Position p = new Position();
            p.Current = pos;
            Parameter = p;
        }

        public override string ToString(IFormatProvider formatProvider)
        {
            string result = _format;
            foreach (DictionaryEntry item in _parameters)
                result = result.Replace((string)item.Key, (string)_parameters[item.Key]);
            if (_arguments?.ToArray() is object[] args)
                return string.Format(formatProvider, result, args);
            else
                return string.Format(formatProvider, result);
        }

        public string GetCommand(RequestOptions format = RequestOptions.JsonAuto)
        {
            return ToString(null) + format.TrimSnakes();
        }

        public override string GetCommand()
            => GetCommand(RequestOptions.JsonAuto);

        public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            foreach (DictionaryEntry de in _parameters)
                yield return KeyValuePair.Create<object, object>(de.Key, de.Value);
        }
    }
}

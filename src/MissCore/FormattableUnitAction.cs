using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using MissBot.Abstractions;
using MissBot.Abstractions.Utils;

namespace MissCore
{
    public class FormattableUnitAction : FormattableString
    {
        private string _format;
        private readonly List<object> _arguments;
        public Position? Parameter;
        private readonly Lazy<ListDictionary> _parameters = new Lazy<ListDictionary>();
        public uint BackParameter
            => Parameter.Value.Back;
        public uint ForwardParameter
            => Parameter.Value.Forward;
        public uint ParameterIndex
            => Parameter.Value.Current;

        internal FormattableUnitAction(string format)
            => _format = format;
        public string this[uint index]
            => _parameters.Value.Keys.Cast<string>().ElementAtOrDefault(Convert.ToInt32(index));

        internal FormattableUnitAction(string format, IEnumerable<object> args = default) : this(format)
        {
            if (args != null)
                _arguments = new List<object>(args);
        }
        internal FormattableUnitAction(string format, IEnumerable<string> parameters = default) : this(format)
        {
            if (parameters != null)
                foreach (var p in parameters)
                    _parameters.Value.Add("@" + p, null);
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
            => _arguments?.Count ?? _parameters?.Value.Count ?? 0;
        public string CurrentParameterName
            => this[Parameter.Value.Current];
        public override object GetArgument(int index)
            => _arguments[index];
        public object this[string key]
        {
            get => _parameters.Value[key];
            set => _parameters.Value[key] = value;
        }
        public void InitParameterPosition()
        {
            if (Parameter.HasValue)
                return;

            byte pos = 0;
            foreach (DictionaryEntry de in _parameters.Value)
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
            foreach (DictionaryEntry item in _parameters?.Value)
                _format = _format.Replace((string)item.Key, (string)_parameters.Value[item.Key]);
            if (_arguments?.ToArray() is object[] args)
                return string.Format(formatProvider, _format, args);
            else
                return string.Format(formatProvider, _format);
        }

        public virtual string GetCommand(RequestOptions format = RequestOptions.JsonAuto)
        {
            return ToString(null) + format.TrimSnakes();
        }
    }
}

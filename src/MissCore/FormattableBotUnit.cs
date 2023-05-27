using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using MissBot.Abstractions;
using MissBot.Abstractions.Utils;

namespace MissCore
{
    public class FormattableBotUnit : FormattableString
    {
        private string _format;
        private readonly List<object> _arguments;
        private readonly Lazy<ListDictionary> _parameters = new Lazy<ListDictionary>();
        internal FormattableBotUnit(string format)
            => _format = format;
        
        public string this[uint index]
            => _parameters.Value.Keys.Cast<string>().ElementAtOrDefault(Convert.ToInt32(index));

        internal FormattableBotUnit(string format, IEnumerable<object> args = default) : this(format)
        {
            if (args != null)            
                _arguments = new List<object>(args);            
        }
        
        internal FormattableBotUnit(string format, IEnumerable<string> parameters = default) : this(format)
        {      
            if (parameters != null)
                foreach (var p in parameters)               
                    _parameters.Value.Add("@" + p, p);            
        }

        internal static FormattableBotUnit Create(string format)
           => new FormattableBotUnit(format);
        internal static FormattableBotUnit Create(string format, params object[] args)
            => new FormattableBotUnit(format, args);
        internal static FormattableBotUnit Create(string format, params string[] args)
            => new FormattableBotUnit(format, args);

        public override string Format
            => _format;
        public override object[] GetArguments()
            => _arguments.ToArray();
        public override int ArgumentCount
            => _arguments?.Count ?? _parameters?.Value.Count ?? 0;
        public override object GetArgument(int index)
            => _arguments[index];
        public object this[string key]
        {
            get => _parameters.Value[key];
            set => _parameters.Value[key] = value;
        }

        public override string ToString(IFormatProvider formatProvider)
        {
            foreach (DictionaryEntry item in _parameters?.Value)
                _format = _format.Replace((string)item.Key, (string)_parameters.Value[item.Key]);
                if (_arguments?.ToArray() is object[] args)
                return string.Format(formatProvider, _format);
            else
                return string.Format(formatProvider, _format);
        }

        public virtual string GetCommand(RequestOptions format = RequestOptions.JsonAuto)
        {
            return base.ToString() + format.TrimSnakes();
        }
    }
}

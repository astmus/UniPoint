using System.Collections.Specialized;
using MissBot.Abstractions;

namespace MissCore
{
    public class FormattableBotUnit : FormattableString
    {
        private string _format;
        private readonly List<object> _arguments;
        private readonly Lazy<ListDictionary> _parameters = new Lazy<ListDictionary>();
        internal FormattableBotUnit(string format, IEnumerable<object> info = default)
        {
            _format = format;

            if (info != null)
            {
                _arguments = new List<object>(info);
            }
        }
        internal static FormattableBotUnit Create(string format, params object[] args)
            => new FormattableBotUnit(format, args);

        public override string Format
            => _format;
        public override object[] GetArguments()
            => _arguments.ToArray();
        public override int ArgumentCount
            => _arguments.Count;
        public override object GetArgument(int index)
            => _arguments[index];
        public object this[string key]
        {
            get => _parameters.Value[key];
            set => _parameters.Value[key] = value;
        }

        public override string ToString(IFormatProvider formatProvider)
        {            
            return string.Format(formatProvider, _format, _arguments.ToArray());
        }

        public virtual string GetCommand(RequestOptions format = RequestOptions.JsonAuto)
        {
            return base.ToString() + format.TrimSnakes();
        }
    }
}

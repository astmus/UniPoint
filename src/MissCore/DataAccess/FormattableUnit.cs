using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Utils;

namespace MissCore.DataAccess
{
	public class FormattableUnit : FormattableUnitBase
	{
		private string _format;
		private readonly List<object> _arguments;
		public Position Parameter;
		private readonly ListDictionary _parameters = new ListDictionary();

		public IEnumerable<string> ParameterNames
			=> _parameters.Keys.Cast<string>();

		public int ParameterIndex
			=> Parameter.Current;
		protected FormattableUnit() { }
		internal FormattableUnit(string format)
			=> _format = format;

		internal FormattableUnit(string format, IEnumerable<object> args = default) : this(format)
		{
			if (args != null)
				_arguments = new List<object>(args);
		}
		internal FormattableUnit(string format, IEnumerable<string> parameters = default) : this(format)
		{
			if (parameters != null)
				foreach (var p in parameters)
					_parameters.Add(p, null);
		}

		internal static FormattableUnit Create(string format)
		   => new FormattableUnit(format);
		internal static FormattableUnit Create(string format, params object[] args)
			=> new FormattableUnit(format, args);
		internal static FormattableUnit Create(string format, params string[] args)
			=> new FormattableUnit(format, args);

		public override string Format
			=> _format;
		public override object[] GetArguments()
			=> _arguments.ToArray();
		public override int ArgumentCount
			=> _arguments?.Count ?? _parameters?.Count ?? 0;
		public string CurrentParameterName
			=> ParameterNames.ElementAtOrDefault(Convert.ToInt32(Parameter.Current));
		public override object GetArgument(int index)
			=> _parameters[index];

		public object this[string key]
		{
			get => _parameters[key];
			set => _parameters[key] = value;
		}
		public void SetupParameterPosition()
		{
			Parameter = new Position();
			foreach (DictionaryEntry de in _parameters)
				if (de.Value != null)
					Parameter.Forward();
				else
					break;
		}

		public override string ToString(IFormatProvider formatProvider)
		{
			var result = Format;
			foreach (DictionaryEntry item in _parameters)
				result = result.Replace("@" + (string)item.Key, (string)_parameters[item.Key]);
			if (_arguments?.ToArray() is object[] args)
				return string.Format(formatProvider, result, args);
			else
				return string.Format(formatProvider, result);
		}

		public override string GetCommand()
			=> ToString(CultureInfo.CurrentCulture);

		public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			foreach (DictionaryEntry de in _parameters)
				yield return KeyValuePair.Create<object, object>(de.Key, de.Value);
		}

		protected override IEnumerator<KeyValuePair<object, object>> GetThisEnumerator()
			=> this.GetEnumerator();
	}
}

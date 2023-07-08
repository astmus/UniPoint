using System.Collections;
using MissBot.Abstractions;

namespace MissBot.Abstractions.Bot
{
    public abstract class FormattableUnitBase : FormattableString, IEnumerable<KeyValuePair<object, object>>
    {
        public abstract string GetCommand();
        public abstract IEnumerator<KeyValuePair<object, object>> GetEnumerator();
        protected abstract IEnumerator<KeyValuePair<object, object>> GetThisEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetThisEnumerator();
    }
}

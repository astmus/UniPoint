using System.Collections;
using MissBot.Abstractions;

namespace MissBot.Abstractions.Entities
{
    public abstract class FormattableUnitActionBase : FormattableString, IEnumerable<KeyValuePair<object, object>>
    {
        public abstract string GetCommand();
        public abstract IEnumerator<KeyValuePair<object, object>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

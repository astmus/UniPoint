using System.Collections.Concurrent;
using MissBot.Identity;

namespace MissCore.Data.Context
{
	public class Cache : ConcurrentDictionary<string, object>
	{
		public T Get<T>(string identifier = default)
		{
			string id = identifier ?? Id<T>.Instance;
			var result = default(T);
			if (TryGetValue(id, out var o) && o is T val) return val;

			return result;
		}
		public T Get<T>(Id<T> identifier)
			=> Get<T>(identifier.Key);

		public T Get<T>(Id identifier)
			=> Get<T>(identifier.Key);

		public TAny Any<TAny>()
		{
			return this.Where(x => x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();
		}

		public T Set<T>(T value, string identifier = default)
			=> (T)AddOrUpdate(identifier ?? Id<T>.Instance,
					(k, w) => w,
					(k, o, w) => this[k] = w, value);

		public T Set<T>(T value, Id identifier)
			=> Set<T>(value, identifier.Key);
	}
}






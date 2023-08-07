using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using MissBot.Abstractions;
using MissBot.Identity;
using MissCore.Data.Collections;

namespace MissCore.Data.Context
{
	public class Context : ConcurrentDictionary<string, object>, IContext
	{
		public DataMap Map { get; protected set; }
		public Context() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		public T Get<T>(string id)
		{
			var result = default(T);
			if (TryGetValue(id, out var o) && o is T val)
				return val;

			result = Map.ReadObject<T>(id);
			return Set(result, id);
		}

		public T Get<T>(T defaultValue = default, Id identifier = default)
		{
			string id = identifier?.Value ?? Id<T>.Instance.Value;
			var result = default(T);
			if (TryGetValue(id, out var o) && o is T val)
				return val;

			result = Map.ReadObject<T>(id);
			return Set(result ?? defaultValue, id);
		}

		public T Take<T>([CallerMemberName] string name = default)
		{
			var id = name;
			var result = default(T);
			if (TryGetValue(id, out var r) && r is T reference)
				return reference;

			result = Map.ReadObject<T>(id);
			return result;
		}

		public TAny Any<TAny>()
			=> this.Where(x => x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();

		public T Set<T>(T value, string name = null)
			=> (T)AddOrUpdate(name ?? Id<T>.Instance,
					(k, w) => w,
					(k, o, w) => this[k] = w, value);

		public T Take<T>(Id<T> identifier)
			=> Take<T>(identifier.Value);
	}

}






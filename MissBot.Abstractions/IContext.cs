using System.Runtime.CompilerServices;
using MissBot.Identity;

namespace MissBot.Abstractions
{
	public interface IContext
	{
		object this[string index] { get; }
		//T TakeByKey<T>();
		T Take<T>([CallerMemberName] string name = default);
		T Take<T>(Id<T> identifier);
		T Get<T>(T defaultValue = default, Id identifier = default);
		T Get<T>(string id);
		TAny Any<TAny>();
		T Set<T>(T value, string id = null);
	}
}

using System.Collections;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using MissCore.Data;
using MissCore.DataAccess;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{
	public record ReadUnit : UnitRequest
	{
		public IUnitRequest Read<TEntity>() where TEntity : class
			=> this with { Format = string.Format(Format, DataUnit<TEntity>.Key) };
		public override string ToString()
			=> Format;
		public override string Get(params object[] args)
			=> ToString();
	}

	public class MutableUnit : JObject, IUnitEntity
	{
		public MutableUnit(JObject other) : base(other)
		{

		}
		IEnumerator<string> Sequence;
		public void MutateWithObject<T>(T value)
		{
			//this[Id<T>.Instance] = ;			
			//Add(Sequence.Current, JToken.FromObject(value));
			Merge(value, new JsonMergeSettings() { MergeArrayHandling = MergeArrayHandling.Concat, PropertyNameComparison = StringComparison.InvariantCultureIgnoreCase });
		}
		public void MutateWithValue<T>(T value) where T : struct
		{
		}
	}
}

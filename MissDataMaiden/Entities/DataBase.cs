using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissCore.Bot;
using MissCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Entities
{
	public record Info : DataUnit<DataBase>, IInteractableUnit<DataBase>
	{
		public override object Identifier
			=> Id;
		//    => Id<DataBase>.Instance.Combine(UnitKey, EntityKey, Id).Key;

		[Column]
		[JsonProperty]
		public long Id { get; set; }

		public string DBName { get; set; }
		public string Status { get; set; }
		public string State { get; set; }
		public int DataFiles { get; set; }
		public int DataMB { get; set; }
		public int LogFiles { get; set; }
		public int LogMB { get; set; }
		public string RecoveryModel { get; set; }
		public string LastBackup { get; set; }
		public string IsReadOnly { get; set; }
		//public IUnitActionsSet UnitActions { get; set; } = new UnitActions<DataBase>();
		//[JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
		//public IEnumerable<IEnumerable<IUnitAction<DataBase>>> Actions { get; set; }
	}

	[JsonObject(memberSerialization: MemberSerialization.OptOut, MissingMemberHandling = MissingMemberHandling.Ignore)]
	//[Table("##DataBase")]
	public record DataBase : Unit
	{
		[JsonProperty(Order = int.MinValue)]
		public override string Unit { get; set; } = nameof(DataBase);

		//[Column]
		public string Id { get; set; }

		//[Column("Id")]
		//public override object Identifier
		//	=> Id;

		//[Column]
		public string Name { get; set; }
		//[Column]
		//public int DaysAgo { get; set; }
		//[Column]
		public float Size { get; set; }
		//[Column]

		public string Created { get; set; }
		//public UnitActions UnitActions { get; set; }
		//public override string Id { get => Get<string>(); set => Set(value); }
		//public string Name { get => Get<string>(); set => Set(value); }
		//public float? Size { get => Get<float>(); set => Set(value); }
		//public string Created { get => Get<string>(); set => Set(value); }
	}

	[JsonObject]
	public record Progress([JsonProperty(Order = int.MinValue)] byte Completed, [JsonProperty(Order = int.MinValue + 1)] TimeSpan Elapsed) { }

	public record DataBaseBotUnit : BotUnit<DataBase>, IUnit<DataBase>
	{

	}
}

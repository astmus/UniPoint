using MissBot.Abstractions;
using MissCore.Data;
using MissDataMaiden.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Actions
{
    public record DBRestore : UnitAction<DataBase>;
    public record DBDelete : UnitAction<DataBase>;
    public record DBInfo : UnitAction<DataBase>;
    public record DataBaseDetail(string DBName, string Status, string State, int DataFiles, int DataMB,
                                            [JsonProperty] int LogFiles, int LogMB, string RecoveryModel, string Created, string LastBackup, string IsReadOnly,
                                            [JsonProperty] List<UnitAction<DataBase>> Commands
    ) : UnitAction<DataBase>
    {
        public DataBaseDetail() : this(default, default, default, default, default, default, default, default, default, default, default, default)
        {

        }
        public DataBaseDetail(DataBaseDetail d) : base(d)
        {

        }
    }

}

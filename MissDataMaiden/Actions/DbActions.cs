using MissBot.Abstractions;
using MissCore.Data;
using MissDataMaiden.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Commands
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

    public class DdActionHandler : BaseHandler<InlineEntityAction<DataBase>>, IAsyncHandler<DBDelete>, IAsyncHandler<DBRestore>, IAsyncEntityActionHandler<DBInfo>
    {
        public Task HandleActionAsync(DBInfo action, IHandleContext context, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task HandleAsync(DBDelete data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task HandleAsync(DBRestore data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task HandleAsync(DBInfo data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public override Task HandleAsync(InlineEntityAction<DataBase> data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
    }

}

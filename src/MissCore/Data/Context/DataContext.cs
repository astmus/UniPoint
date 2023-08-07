using MissBot.Abstractions;

namespace MissCore.Data.Context
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class UnitDataContext<TData> : DataUnit<TData>.UnitContext, IUnitContext where TData : class
    {

        public UnitDataContext(TData data) : base(data)
        {
        }
    }

    //public abstract record BaseUnit : BaseItem, IUnit
    //{
    //    [JsonProperty("Unit", Order = int.MinValue)]
    //    public virtual string UnitKey { get; set; }

    //    public abstract IEnumerator UnitEntities { get; }
    //    protected IMetaData Metadata { get; set; }
    //    protected JToken UnitData { get; set; }
    //    //public virtual string EntityKey { get; set; }

    //    public virtual void SetRawData<TUnitData>(TUnitData data) where TUnitData : JToken
    //        => UnitData = data;

    //    protected abstract T Get<T>([CallerMemberName] string name = null);
    //    protected abstract void Set<T>(T item, [CallerMemberName] string name = null);

    //}

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    //public abstract record BaseItem : IIdentible
    //{
    //    public abstract object Identifier { get; }
    //}

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    //public abstract record BaseBotAction : BaseItem, IBotAction
    //{
    //    public override object Identifier
    //        => string.Join('.', Unit, Action);

    //    public virtual string Unit { get; set; }

    //    [JsonProperty("text", Required = Required.Always, Order = int.MinValue)]
    //    public virtual string Action { get; set; }

    //    public virtual string EntityKey
    //        => Action;
    //}
}

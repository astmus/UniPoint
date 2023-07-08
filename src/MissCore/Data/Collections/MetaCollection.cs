using System.Collections.Generic;
using System.Linq;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissCore.Data.Entities;
using Newtonsoft.Json.Linq;


namespace MissCore.Data.Collections
{
    [JsonArray]
    public class MetaCollection<TData> : MetaCollection, IMetaCollection<TData> where TData : class
    {
        public readonly static new MetaCollection<TData> Empty = new MetaCollection<TData>();
        #region Constructors
        public MetaCollection(params object[] content) : base(content) { }
        public MetaCollection(IEnumerable<JToken> items) : base(items)
        {
        }

        public MetaCollection() : base()
        {
        }

        public MetaCollection(JArray items) : base(items)
        {
        }

        public MetaCollection(object content) : base(content)
        {
        }
        #endregion

        protected override void Init()
        {
            if (ChildrenTokens.FirstOrDefault() is JToken token)
                MetaData = Unit<TData>.ReadMetadata<TData>(token);
        }
        public IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity : class, TData
        {
            foreach (var token in this)
            {
                var result = token.ToObject<TEntity>();
                //if (result is Unit unit)
                //    unit.Metadata = Unit.MetaData.FromRootToken<TEntity>(token);// Setup(token, out unit, Data);
                yield return result;
            }
        }

        //public override IEnumerable<IMetaValue> GetValues()
        //{
        //    foreach (var token in this)
        //    {
        //        Metadata.SetRoot(token);
        //        foreach (var item in Metadata.Items)
        //            yield return new MetaValue(item, Metadata.GetValue(item));
        //    }
        //}
        public IEnumerable<IUnit<TData>> EnumarateUnits()
        {
            foreach (var token in this)
            {
                var result = token.ToObject<Unit<TData>>();
                yield return default;//result.DataContext[MetaData];
                //if (result is Unit unit)
                //    unit.Metadata = Unit.MetaData.FromRootToken<TUnit>(token);
                // result.Entity = token.ToObject<TUnit>();
                //Metadata.SetContainer(token);
                //Unit.MetaData.Setup(token, out result, Metadata);
                //yield return result;
            }
        }
        public IEnumerable<TUnit> EnumarateUnits<TUnit>() where TUnit : BaseUnit, IUnit<TData>
        {
            foreach (var token in this)
            {
                var result = token.ToObject<TUnit>();
                result.SetContext(token);
                //if (result is Unit unit)
                //    unit.Metadata = Unit.MetaData.FromRootToken<TUnit>(token);
                // result.Entity = token.ToObject<TUnit>();
                //Metadata.SetContainer(token);
                //Unit.MetaData.Setup(token, out result, Metadata);
                yield return result;
            }
        }

        IEnumerator<TData> IEnumerable<TData>.GetEnumerator()
        {
            return SupplyTo<TData>().GetEnumerator();
        }
    }

    public class MetaCollection : JArray, IMetaCollection, IList<JToken>
    {
        public readonly static MetaCollection Empty = new MetaCollection();

        public IMetaData MetaData { get; protected set; }
        public string UnitKey
            => this.FirstOrDefault()?.Value<string>() ?? nameof(MetaCollection);
        public MetaCollection(IEnumerable<JToken> items) : base(items.ToArray())
            => Init();

        public MetaCollection(JArray items) : base(items)
            => Init();

        public MetaCollection()
            => Init();

        public MetaCollection(params object[] content) : base(content)
            => Init();

        public MetaCollection(object content) : base(content)
            => Init();

        protected virtual void Init()
            => MetaData = Unit.ReadMetadata(this.FirstOrDefault());

        protected virtual IEnumerable<TSub> SupplyTo<TSub>() where TSub : class
        {

            foreach (var token in this)
            {
                //Metadata. .SetRoot(token);
                //var result = (Metadata as Unit.MetaData). Clone<TSub>();
                //if (result is Unit unit)
                //    unit.Metadata = Unit.MetaData.FromRootToken<TSub>(token);
                //if (result is Unit unit)
                //    Unit.MetaData.Setup(token, out unit, Data);
                yield return token.ToObject<TSub>();
            }
        }

        //public IEnumerable<TUnit> BringTo<TUnit>() where TUnit : class
        //{
        //    foreach (var item in tokens)
        //    {
        //        Metadata.SetContainer(item);
        //        yield return Metadata.SupplayTo<TUnit>();
        //    }
        //}

        public virtual IEnumerable<TUnit> Enumarate<TUnit>() where TUnit : class
        {
            foreach (var token in this)
            {
                var result = token.ToObject<TUnit>();
                if (result is ResultUnit<TUnit> unit)
                    unit.SetContext(unit);// = Unit<TUnit>.ReadMetadata(unit.Entity);
                yield return result;
            }
        }
        //public IEnumerable<string> GetEnumerator()
        //{
        //    foreach (var item in tokens)
        //    {
        //        Metadata.SetContainer(item);
        //        yield return Metadata.StringValue;
        //    }
        //}


    }
}



using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissCore.Data.Context;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{
    [JsonObject]
    public record Unit(IContext<Unit> Context = default)    : IUnit
    {
        public MetaData Meta { get; internal set; }
        protected T Get<T>()
            => Context.Get<T>();
        protected T Set<T>(T value, [CallerMemberName] string name = null)
            => Context.Set(value, name);

    }
    [JsonObject]
    public record Unit<TEntity>(TEntity Entity = default) : Unit, IUnit<TEntity>
    {
        public static readonly TEntity Sample = Activator.CreateInstance<TEntity>();
        public static readonly string Key = typeof(TEntity).Name;
        public static DataMap JoinData(TEntity entity, DataMap map)
        {
            map ??= new DataMap(entity);
            map.Parse(entity);
            return map;
        }
        public class Collection : List<TEntity> { }
        
        public static string Stringify(string[] items)
            => string.Join('\t', from s in items
                                 where s.Length > 2 && !s.EndsWith("= ")
                                 select s);
        public static string ParseTyped(object value)
            => Stringify(value.ToString().Split('{', ':', ',', '}'));

  
    }
}

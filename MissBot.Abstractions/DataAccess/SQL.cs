using MissBot.Abstractions;

namespace MissBot.DataAccess.Sql
{
    public enum SQLJson
    {
        Auto,
        Path,
    }
    public record SQL(string Sql = default) : Unit
    {
        const string AppendAuto = "{0} FOR JSON AUTO , ROOT('Content')";
        const string AppendPath = "{0} FOR JSON PATH , ROOT('Content')";
        public SQLJson Type { get; set; }
        public static explicit operator SQL(string sql)
        => new SQL(sql);
        public virtual string Command
            => Type switch {
                SQLJson.Auto => string.Format(AppendAuto, Sql),
                SQLJson.Path => string.Format(AppendPath, Sql),
              _ => Sql
            };
    }   


    
}

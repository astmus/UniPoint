using System.Runtime.CompilerServices;
using MissBot.Abstractions;

namespace MissBot.DataAccess.Sql
{
    public enum SQLJson
    {
        Auto,
        Path,
    }

    public struct SQL : IFormattable, IFormatProvider, ICustomFormatter
    {
        public SQL(string value)
        {
            Sql = value;
        }
        const string AppendAuto = "{0} FOR JSON AUTO , ROOT('Content')";
        const string AppendPath = "{0} FOR JSON PATH , ROOT('Content')";
        const string Empty = "SELECT * FROM SYS.TABLES ";
        const string SelectFrom = "SELECT {0} FROM ##{1} ";
        const string SelectAllFrom = "SELECT * FROM ##{0} ";
        public string Sql { get; set; } = Empty;
        public bool IsEmpty
            => string.IsNullOrEmpty(Sql);
        public SQLJson Type { get; set; } = SQLJson.Path;
        public static implicit operator SQL(string sql)
            => new SQL(sql);
        public string Command
            => Type switch {
                SQLJson.Auto => string.Format(AppendAuto, Sql),
                SQLJson.Path => string.Format(AppendPath, Sql),
              _ => Sql
            };
        public static SQL Parse<TEntity>(TEntity entity)
            => Stringify( Convert.ToString(entity).Split('{', ',', '}').AsMemory());
        internal static string Stringify(Memory<string> items)
         => string.Format(SelectAllFrom, items.Span[0]);
        // => string.Format(SelectFrom, string.Join(',', items[1..].ToArray()), items.Span[0]).Replace(" = ", "").Replace(" , "," ");
        //)
        //from s in items
        //                                where s.Length > 2 && !s.EndsWith("= ")
        //                                select s);
        internal static string Stringify(string[] items)
        => string.Join(Environment.NewLine, from s in items
                                            where s.Length > 2 && !s.EndsWith("= ")
                                            select s);

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }

        public object? GetFormat(Type? formatType)
        {
            throw new NotImplementedException();
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }
    }   


    
}

using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    [Flags]
    public enum SQLType
    {
        Raw = 0,
        JSONPath = 1,
        JSONAuto = 4,
        JSONRoot = 8,
        JSONNoWrap = 16
    }

    public struct SQLCommand : IFormattable, IFormatProvider, ICustomFormatter
    {
        public SQLCommand(string value, IEnumerable<string> fileds = default, SQLType type = SQLType.JSONPath | SQLType.Raw)
        {
            Sql = value;
            Fields = fileds;
            Type = type;
        }
        public IEnumerable<string> Fields { get; set; }
        public string Sql { get; set; } = SQL.Empty;
        public SQLType Type;

        public static implicit operator SQLCommand(string sql)
            => new SQLCommand(sql);
        public static implicit operator string(SQLCommand cmd)
            => cmd.Command;
        public string Command
            => Type switch
            {
                SQLType.JSONAuto => SQL.Templated(SQL.Templates.JSONAuto, Sql).ApplyFields(Fields),
                SQLType.Raw => SQL.Templated(SQL.Templates.JSONAuto, Sql).ApplyFields(Fields),
                SQLType.JSONPath | SQLType.JSONNoWrap => SQL.Templated(SQL.Templates.JSONPath + SQL.JSONNoWrap, Sql).ApplyFields(Fields),
                SQLType.JSONPath => SQL.Templated(SQL.Templates.JSONPath, Sql).ApplyFields(Fields),
                _ => Sql
            };
       
        #region InterfaceImplementation
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
        #endregion
    }

    public static class SQL
    {
        public const string Empty = "SELECT 1";
        public const string SelectFrom = "SELECT * FROM ##";
        public const string AllUnits = $"SELECT * FROM ##{nameof(BotUnit)}s ";
        public const string FirstUnit = $"SELECT TOP 1 * FROM ##{nameof(BotUnit)}s ";
        public const string JSONNoWrap = ", WITHOUT_ARRAY_WRAPPER";             

        public static readonly string[] AllFileds = { "*" };
        public static class Templates
        {
            public const string SelectAllFrom = "SELECT * FROM ##{0}";
            public const string SelectFrom = "SELECT {1} FROM ##{0}";
            public const string JSONAuto = "{0} FOR JSON AUTO";
            public const string JSONPath = "{0} FOR JSON PATH";
            public const string JSONRoot = ", ROOT('{0}')";
        }
        public static SQLCommand Entities<TEntity>(FieldNamesSelector<TEntity> fields = default)
            => new SQLCommand($"{SQL.AllUnits} WHERE Entity = '{Unit<TEntity>.EntityName}'", fields?.Invoke(default(TEntity)));// .Replace("*", fields.SqlFields()));
        public static SQLCommand Entity<TEntity>(FieldNamesSelector<TEntity> fields = default)
            => new SQLCommand($"{SQL.FirstUnit} WHERE Entity = '{Unit<TEntity>.EntityName}'", fields?.Invoke(default(TEntity)), SQLType.JSONPath | SQLType.JSONNoWrap);
        public static SQLCommand Command<TCommand>(IEnumerable<string> fields = default) where TCommand : BotCommand
            => new SQLCommand($"{SQL.FirstUnit} WHERE Entity = '{Unit<BotCommand>.EntityName}' AND Command = '/{BotCommand<TCommand>.Name}'", fields);
        public static SQLUnit<TCommand> CommandUnit<TCommand>(IEnumerable<string> fields = default) where TCommand : BotCommand
            => new SQLUnit<TCommand>(new SQLCommand($"{SQL.FirstUnit} WHERE Entity = '{Unit<BotCommand>.EntityName}' AND Command = '/{BotCommand<TCommand>.Name}'", fields));
        public static SQLCommand Parse(string sql)
            => sql;
        public static string Templated(string template, string sql)
            => string.Format(template, sql);
        public static SQLCommand Parse<TEntity>(TEntity entity)
            => Stringify(Convert.ToString(entity).Split('{', ',', '}').AsMemory());
        internal static string Stringify(Memory<string> items)
            => string.Format(SelectFrom, items.Span[0]);
        internal static string Stringify(string[] items)
            => string.Join(Environment.NewLine, from s in items
                                            where s.Length > 2 && !s.EndsWith("= ")
                                            select s);
    }
    public static class SQLExtensions
    {
        public static string SqlFields(this IEnumerable<string> selectFields)
            => string.Join(',', selectFields);
        public static string ApplySqlFields(this IEnumerable<string> selectFields, string sql)
            => sql.Replace("*", selectFields.SqlFields());
        internal static string ApplyFields(this string sql, IEnumerable<string> fields) => fields?.Count() switch
        {
            > 1 => fields.ApplySqlFields(sql),
            _ => sql
        };
    }
}

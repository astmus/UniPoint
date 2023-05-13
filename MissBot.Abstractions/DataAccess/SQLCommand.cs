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
    public struct SQLCommand<TEntity> : IRepositoryCommand
    {
        public string Entity
             => Unit<TEntity>.UnitName;
        public SQLType Type;
        public string WHERE;
        string Template
            => $"{SqlUnit.Templated(SqlUnit.Templates.Entity,Entity)} {WHERE}";
        public SQLCommand(string where, SQLType type = SQLType.JSONAuto | SQLType.JSONNoWrap)
        {
            WHERE = $" WHERE {where} ";
            Type = type;
        }

        public static implicit operator string(SQLCommand<TEntity> cmd)
            => cmd.Command;
        public string Command
            => Type switch
            {

                SQLType.JSONAuto => SqlUnit.Templated(SqlUnit.Templates.JSONAuto, Template),//.ApplyFields(Fields),
                SQLType.Raw => SqlUnit.Templated(SqlUnit.Templates.JSONAuto, Template),//.ApplyFields(Fields),
                SQLType.JSONPath | SQLType.JSONNoWrap => SqlUnit.Templated(SqlUnit.Templates.JSONPath + SqlUnit.JSONNoWrap, Template),//.ApplyFields(Fields),
                SQLType.JSONAuto | SQLType.JSONNoWrap => SqlUnit.Templated(SqlUnit.Templates.JSONAuto + SqlUnit.JSONNoWrap, Template),
                SQLType.JSONPath => SqlUnit.Templated(SqlUnit.Templates.JSONPath, Template),//.ApplyFields(Fields),
                _ => Entity
            };
    }

    public struct SQLCommand : IFormattable, IFormatProvider, ICustomFormatter, IRepositoryCommand
    {
        public SQLCommand(string value, IEnumerable<string> fileds = default, SQLType type = SQLType.JSONPath | SQLType.Raw)
        {
            Sql = value;
            Fields = fileds;
            Type = type;
        }
        public IEnumerable<string> Fields { get; set; }
        public string Sql { get; set; } = SqlUnit.Empty;
        public SQLType Type;

        public static implicit operator SQLCommand(string sql)
            => new SQLCommand(sql);
        public static implicit operator string(SQLCommand cmd)
            => cmd.Command;
        public string Command
            => Type switch
            {
                SQLType.JSONAuto => SqlUnit.Templated(SqlUnit.Templates.JSONAuto, Sql).ApplyFields(Fields),
                SQLType.Raw => SqlUnit.Templated(SqlUnit.Templates.JSONAuto, Sql).ApplyFields(Fields),
                SQLType.JSONPath | SQLType.JSONNoWrap => SqlUnit.Templated(SqlUnit.Templates.JSONPath + SqlUnit.JSONNoWrap, Sql).ApplyFields(Fields),
                SQLType.JSONPath => SqlUnit.Templated(SqlUnit.Templates.JSONPath, Sql).ApplyFields(Fields),
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

    public static class SqlUnit
    {
        public const string Empty = "SELECT 1";
        public const string SelectFrom = "SELECT * FROM ##";
        public const string RootUnit = $"SELECT * FROM ##{nameof(BotUnit)}s ";
        public const string SelectFirst = $"SELECT TOP 1 * FROM ##{nameof(BotUnit)}s ";
        public const string JSONNoWrap = ", WITHOUT_ARRAY_WRAPPER";

        public static readonly string[] AllFileds = { "*" };
        public static class Templates
        {
            public const string SelectAllFrom = "SELECT * FROM ##{0}";
            public const string SelectFrom = "SELECT {1} FROM ##{0}";
            public const string JSONAuto = "{0} FOR JSON AUTO";
            public const string JSONPath = "{0} FOR JSON PATH";
            public const string JSONRoot = ", ROOT('{0}')";
            public const string Entity = "SELECT * FROM ##{0} [{0}] INNER JOIN ##BotUnits Commands ON Commands.Entity = [{0}].EntityName";
        }

        public static SQLCommand Entities<TEntity>(FieldNamesSelector<TEntity> fields = default) where TEntity : class
            => new SQLCommand($"{RootUnit} WHERE Entity = '{Unit<TEntity>.UnitName}'", fields?.Invoke(default(TEntity)));
        public static BotRequest Actions<TAction>() where TAction : BotActionRequest
            => new BotRequest($"{RootUnit} WHERE Entity = '{Unit<TAction>.UnitName}'", SQLType.JSONPath | SQLType.JSONNoWrap);
        public static SQLCommand Entity<TEntity>(FieldNamesSelector<TEntity> fields = default)
            => new SQLCommand($"{SelectFirst} WHERE Entity = '{Unit<TEntity>.UnitName}'", fields?.Invoke(default(TEntity)), SQLType.JSONPath | SQLType.JSONNoWrap);
        public static SQLCommand Command<TCommand>(IEnumerable<string> fields = default) where TCommand : BotCommand
            => new SQLCommand($"{SelectFirst} WHERE Entity = '{Unit<BotCommand>.UnitName}' AND Command = '/{Abstractions.Entities.BotUnit2<TCommand>.EntityName}'", fields, SQLType.JSONPath | SQLType.JSONNoWrap);
        public static SQLCommand Select<TUnitData>(string field, string value)
            => new SQLCommand($"{SelectFrom}{Unit<TUnitData>.UnitName} WHERE {field} = '{value}'", null, SQLType.JSONPath | SQLType.JSONNoWrap);

        //public static SQLUnit<TCommand> CommandUnit<TCommand>(IEnumerable<string> fields = default) where TCommand : BotCommand
        //    => new SQLUnit<TCommand>(new SQLCommand($"{FirstUnit} WHERE Entity = '{Unit<BotCommand>.EntityName}' AND Command = '/{BotCommand<TCommand>.Name}'", fields, SQLType.JSONPath | SQLType.JSONNoWrap));
        //public static SQLCommand Parse(string sql)
        //    => sql;

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
        //public static string ToWhere(this MetaData data)
        //    => string.Join("AND", data.KeyPairs.Select(s => s.WhereTemplate()));

        internal static string WhereTemplate(this KeyValuePair<string, object> keyValue) => keyValue.Value switch
        {
            string value => $"{keyValue.Key} = '{value}' ",
            _ => $"{keyValue.Key} = {keyValue.Value} ",
        };
        internal static string SqlFields(this IEnumerable<string> selectFields)
            => string.Join(',', selectFields);
        internal static string ApplySqlFields(this IEnumerable<string> selectFields, string sql)
            => sql.Replace("*", selectFields.SqlFields());
        internal static string ApplyFields(this string sql, IEnumerable<string> fields) => fields?.Count() switch
        {
            > 1 => fields.ApplySqlFields(sql),
            _ => sql
        };

    }
}

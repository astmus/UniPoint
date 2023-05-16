using System.Runtime.CompilerServices;
using MissBot.Abstractions;

namespace BotService.Common
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

    public record UnitRequest<TUnit>(string Template, IRequestInformation Info, RequestFormat Type = RequestFormat.JsonAuto) : IRepositoryCommand
    {
        public static implicit operator string(UnitRequest<TUnit> cmd)
            => cmd.Request;
        public string Request
            => Type switch
            {
                RequestFormat.JsonAuto
                    => FormattableStringFactory.Create(Template, Info.Unit, Info.Entity, Info.Criteria.Format.Make()).ToString(),//.ApplyFields(Info.EntityFields),
                RequestFormat.JsonPath
                    => FormattableStringFactory.Create(Template, Info.Unit, Info.Entity, Info.Criteria.Format.Make()).ToString(),
                _ => Template
            };

        public string ToRequest(RequestFormat format = RequestFormat.JsonAuto)
            => ToString(format.TrimSnakes(), default);

        public string ToString(string? format, IFormatProvider? formatProvider)
            => string.Format(format, this);

        public IRepositoryCommand SingleResult()
            => this with { Template = Templates.SelectSingle };
    }


    public class UnitRequestFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region InterfaceImplementation

        public object? GetFormat(Type? formatType)
        {
            // FormattableString message = $"The speed of light is {12:N3} km/s.";
            return this;
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            FormattableString message = $"The speed of light is {12:N3} km/s.";
            return message.ToString();
        }
        #endregion
    }
    public static class Templates
    {
        //const string Select = "SELECT * FROM ##{0}";
        public const string Select = "SELECT {0} FROM ##{1}";
        public const string SelectSingle = "SELECT TOP 1 {0} FROM ##{1}";
        public const string JSONAuto = "{0} FOR JSON AUTO";
        public const string JSONPath = "{0} FOR JSON PATH";
        public const string JSONRoot = ", ROOT('{0}')";
        public const string Entity = "SELECT * FROM ##[{0}] INNER JOIN ##BotUnits Commands ON Commands.Entity = [{0}].EntityName";
        public const string Empty = "SELECT 1";
        public const string From = " FROM ##{0}";
        public const string RootUnit = $"SELECT * FROM ##{nameof(BotUnit)}s ";
        public const string SelectFirst = $"SELECT TOP 1 * FROM ##{nameof(BotUnit)}s ";
        public const string JsonNoWrap = "{0}, WITHOUT_ARRAY_WRAPPER";
        public static readonly string[] AllFileds = { "*" };
    }

    public static class SqlUnit
    {
        public const string Empty = "SELECT 1";
        public const string SelectFrom = "SELECT * FROM ##";
        public const string RootUnit = $"SELECT * FROM ##{nameof(BotUnit)}s ";
        public const string SelectFirst = $"SELECT TOP 1 * FROM ##{nameof(BotUnit)}s ";
        public const string JSONNoWrap = ", WITHOUT_ARRAY_WRAPPER";

        public static string Templated(string template, string sql)
            => string.Format(template, sql);
        internal static string Stringify(Memory<string> items)
            => string.Format(SelectFrom, items.Span[0]);
        internal static string Stringify(string[] items)
            => string.Join(Environment.NewLine, from s in items
                                                where s.Length > 2 && !s.EndsWith("= ")
                                                select s);
    }
    public static class SQLExtensions
    {
        internal static string SqlFields(params string[] selectFields)
            => string.Join(',', selectFields);
        internal static string ApplySqlFields(this IEnumerable<string> selectFields, string sql)
            => string.Format(sql, selectFields);
        internal static string ApplyFields(this string cmd, IEnumerable<string> fields) => fields?.Count() switch
        {
            > 1 => fields.ApplySqlFields(cmd),
            _ => cmd
        };

    }
}

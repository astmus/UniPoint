using System.Runtime.CompilerServices;
using MissBot.Abstractions;

namespace MissCore
{
    public record UnitRequest<TUnit>(string Template, RequestOptions Type = RequestOptions.JsonAuto) : IUnitRequest<TUnit>
    {
        public static implicit operator string(UnitRequest<TUnit> cmd)
            => cmd.GetCommand(RequestOptions.JsonAuto);
        FormattableUnitAction Request { get; set; } = FormattableUnitAction.Create(Template);
        public RequestOptions RequestOptions { get; set; }

        public virtual string ToString(string? format, IFormatProvider? formatProvider)
            => string.Format(format, this);

       public virtual string GetCommand(RequestOptions options)
        {
            if (RequestOptions == RequestOptions.Unknown)
                RequestOptions = options;
            return Request.GetCommand(options);
        }
    }


    public class UnitRequestFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region InterfaceImplementation

        public object? GetFormat(Type? formatType)
        {
            
            return this;
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            FormattableString message = $"The speed of light is {12:N3} km/s.";
            return message.ToString();
        }
        #endregion
    }
    internal static class Templates
    {
        public const string Read = "SELECT {0} FROM ##{1}";
        public const string ReadAllByCriteria = "SELECT * FROM ##{0} {1}";
        public const string ReadAll = "SELECT * FROM ##{0}";
        public const string Search = "SELECT * FROM ##{0} WHERE Name LIKE '%{1}%' ORDER BY Name OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY";
        public const string ReadSingle = "SELECT TOP 1 {0} FROM ##{1}";
        public const string JsonAuto = "{0} FOR JSON AUTO";
        public const string JsonPath = "{0} FOR JSON PATH";
        public const string JsonRoot = ", ROOT('{0}')";
        public const string Entity = "SELECT * FROM ##{0} INNER JOIN ##BotUnits Commands ON Commands.Entity = {0}.EntityName";
        public const string Empty = "SELECT 1";
        public const string From = " FROM ##{0}";
        public const string JsonNoWrap = "{0}, WITHOUT_ARRAY_WRAPPER";
        public static readonly string[] AllFileds = { "*" };
    }
}

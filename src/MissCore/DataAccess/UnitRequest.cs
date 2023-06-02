using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using MissBot.Entities;

namespace MissCore.DataAccess
{
    public record UnitRequest : BotUnit, IUnitRequest
    {
        public UnitRequest()
        {
            DataUnit = FormattableUnit.Create(Template);
        }
        public static implicit operator string(UnitRequest cmd)
            => cmd.GetCommand();

        FormattableUnit DataUnit { get; init; }
        public RequestOptions Options { get; set; } = RequestOptions.JsonAuto;
        [JsonIgnore]
        public IEnumerable<IUnitItem> Params { get; init; }

        public override string ToString()
            => GetCommand();

        public virtual string GetCommand()
        {
            return DataUnit.GetCommand();
        }
    }
    public record UnitRequest<TUnit>(string raw) : UnitRequest, IUnitRequest<TUnit>
    {
        public static implicit operator string(UnitRequest<TUnit> cmd)
            => cmd.GetCommand();
    }


    //internal static class Templates
    //{
    //    public const string Read = "SELECT {0} FROM ##{1}";
    //    public const string ReadAllByCriteria = "SELECT * FROM ##{0} {1}";
    //    public const string ReadAll = "SELECT * FROM ##{0}";
    //    public const string Search = "SELECT * FROM ##{0} WHERE Name LIKE '%{1}%' ORDER BY Name OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY";
    //    public const string ReadSingle = "SELECT TOP 1 {0} FROM ##{1}";
    //    public const string JsonAuto = "{0} FOR JSON AUTO";
    //    public const string JsonPath = "{0} FOR JSON PATH";
    //    public const string JsonRoot = ", ROOT('{0}')";
    //    public const string Entity = "SELECT * FROM ##{0} INNER JOIN ##BotUnits Commands ON Commands.Entity = {0}.EntityName";
    //    public const string Empty = "SELECT 1";
    //    public const string From = " FROM ##{0}";
    //    public const string JsonNoWrap = "{0}, WITHOUT_ARRAY_WRAPPER";
    //    public static readonly string[] AllFileds = { "*" };
    //}
}

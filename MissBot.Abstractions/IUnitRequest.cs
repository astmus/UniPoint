using MissBot.Abstractions.DataAccess;

namespace MissBot.Abstractions
{
    public interface IUnitRequest<TUnit> : IUnitRequest
    {
    }

    public interface IUnitRequest : IRepositoryCommand
    {
        int ArgumentCount { get; }
        string Template { get; }
        object this[string key] { get;set; }
        object GetArgument(int index);
        object[] GetArguments();
        string ToString(IFormatProvider formatProvider);
    }
}

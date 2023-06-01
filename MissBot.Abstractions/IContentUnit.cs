namespace MissBot.Abstractions
{
    public interface IContentUnit<out TEntity>
    {
        IEnumerable<TEntity> Content { get; }
    }
    public interface IUnitContainable<TEntity>
    {
        void Add<TUnit>(TUnit unit) where TUnit : IUnit<TEntity>;
    }
}

namespace MissBot.Abstractions
{
    public interface IContentUnit<TEntity>
    {
        IEnumerable<TEntity> Content { get; }
    }
}
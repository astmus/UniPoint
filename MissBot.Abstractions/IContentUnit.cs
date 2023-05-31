namespace MissBot.Abstractions
{
    public interface IContentUnit<TEntity>
    {
        IEnumerable<TEntity> Content { get; }
    }
    public interface IContent<TEntity>
    {
       TEntity Data { get; set; }
    }
}

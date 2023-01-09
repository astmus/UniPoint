namespace MissCore.DataAccess;
public interface IApplicationGenericRepository
{
    IEnumerable<TEntity> GetRepository<TEntity>() where TEntity : class;
}

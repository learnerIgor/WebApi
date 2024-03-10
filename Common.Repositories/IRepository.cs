using System.Linq.Expressions;

namespace Common.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        TEntity[] GetList(
            int? offset = null,
            int? limit = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool? descending = null);
        TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken);
        int Count(Expression<Func<TEntity, bool>>? predicate = null);
        TEntity Add(TEntity toDo);
        Task<TEntity> AddAsync(TEntity toDo, CancellationToken cancellationToken);
        TEntity Update(TEntity toDo);
        bool Delete(TEntity toDo);
    }
}

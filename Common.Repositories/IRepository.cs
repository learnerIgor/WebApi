using System.Linq.Expressions;

namespace Common.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity[]> GetListAsync(
            int? offset = null,
            int? limit = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool? descending = null,
            CancellationToken cancellationToken = default);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync(TEntity toDo, CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity toDo, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(TEntity toDo, CancellationToken cancellationToken);
    }
}

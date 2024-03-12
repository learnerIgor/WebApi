using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Repositories
{
    public class SqlServerBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SqlServerBaseRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<TEntity[]> GetListAsync(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null,
            bool? descending = null, CancellationToken cancellationToken = default)
        {
            var queryable = _applicationDbContext.Set<TEntity>().AsQueryable();

            if (predicate is not null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy is not null)
            {
                queryable = descending == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
            }

            if (offset.HasValue)
            {
                queryable = queryable.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                queryable = queryable.Take(limit.Value);
            }

            return await queryable.ToArrayAsync(cancellationToken);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? await set.SingleOrDefaultAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? set.Count() : set.Count(predicate);
        }

        public async Task<TEntity> AddAsync(TEntity toDo, CancellationToken cancellationToken)
        {
            var set = _applicationDbContext.Set<TEntity>();
            await set.AddAsync(toDo, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return toDo;
        }

        public async Task<TEntity> UpdateAsync(TEntity bookUpdate, CancellationToken cancellationToken)
        {
            var set = _applicationDbContext.Set<TEntity>();
            set.Update(bookUpdate);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return bookUpdate;
        }

        public async Task<bool> DeleteAsync(TEntity bookDelete, CancellationToken cancellationToken)
        {
            var set = _applicationDbContext.Set<TEntity>();
            set.Remove(bookDelete);
            return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}

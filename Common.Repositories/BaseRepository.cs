using System.Linq.Expressions;

namespace Common.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private static readonly List<TEntity> _data = [];
        public TEntity Add(TEntity toDo)
        {
            _data.Add(toDo);
            return toDo;
        }

        public bool Delete(TEntity toDo)
        {
            return _data.Remove(toDo);
        }

        public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return _data.SingleOrDefault();
            return _data.SingleOrDefault(predicate.Compile());
        }

        public TEntity[] GetList(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null, bool? descending = null)
        {
            var result = _data;

            if (predicate != null)
                result = result.Where(predicate.Compile()).ToList();

            if (orderBy != null)
                result = descending.GetValueOrDefault() ? result.OrderByDescending(orderBy.Compile()).ToList() : result.OrderBy(orderBy.Compile()).ToList();

            result = result.Skip(offset.GetValueOrDefault()).ToList();

            if (limit.HasValue)
                result = result.Take(limit.Value).ToList();

            return result.ToArray();
        }

        public TEntity Update(TEntity toDo)
        {
            Delete(toDo);
            _data.Add(toDo);

            return toDo;
        }

        public int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var entities = _data;
            if (predicate == null)
                return entities.Count();

            return entities.Where(predicate.Compile()).Count();

        }
    }
}

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Persistence.MongoDbRepositories;

public interface IMongoDbRepository<T, in TKey> 
    where T : class, IMongoDbEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(TKey id);
    Task<T> AddAsync(T entity);
    Task<bool> AddRangeAsync(IEnumerable<T> entities);
    Task<T> UpdateAsync(TKey id, UpdateDefinition<T> updateDefinition);
    Task<T> DeleteAsync(T entity);
    Task<T> DeleteAsync(TKey id);
    Task<T> DeleteAsync(Expression<Func<T, bool>> predicate);
    Task<(IEnumerable<T> Items, long TotalCount)> ListAsync(
        Expression<Func<T, bool>> filter = null,
        Expression<Func<T, object>> orderBy = null,
        bool ascending = true,
        int pageIndex = 0,
        int pageSize = 20);
}

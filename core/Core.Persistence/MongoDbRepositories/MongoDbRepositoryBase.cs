using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Persistence.MongoDbRepositories;

public abstract class MongoDbRepositoryBase<T> : IMongoDbRepository<T, string> 
    where T : MongoDbEntity, new()
{
    protected readonly IMongoCollection<T> Collection;
    private readonly MongoDbSettings settings;

    protected MongoDbRepositoryBase(IOptions<MongoDbSettings> options)
    {
        this.settings = options.Value;
        var client = new MongoClient(this.settings.ConnectionString);
        var db = client.GetDatabase(this.settings.Database);
        this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }

    public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
    {
        return predicate == null
            ? Collection.AsQueryable()
            : Collection.AsQueryable().Where(predicate);
    }

    public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return Collection.Find(predicate).FirstOrDefaultAsync();
    }

    public virtual async Task<(IEnumerable<T> Items, long TotalCount)> ListAsync(
        Expression<Func<T, bool>> filter = null,
        Expression<Func<T, object>> orderBy = null,
        bool ascending = true,
        int pageIndex = 0,
        int pageSize = 20
        )
    {
        var query = filter == null ? Collection.AsQueryable() : Collection.AsQueryable().Where(filter);

        var totalCount = await query.CountAsync();

        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public virtual Task<T> GetByIdAsync(string id)
    {
        return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var options = new InsertOneOptions { BypassDocumentValidation = false };
        await Collection.InsertOneAsync(entity, options);
        return entity;
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
    {
        var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
        return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
    }

    public async Task<T> UpdateAsync(string id, UpdateDefinition<T> updateDefinition)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        await Collection.UpdateOneAsync(filter, updateDefinition);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }


    public virtual async Task<T> DeleteAsync(T entity)
    {
        return await Collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
    }

    public virtual async Task<T> DeleteAsync(string id)
    {
        return await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }

    public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.FindOneAndDeleteAsync(filter);
    }
}
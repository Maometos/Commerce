using Commerce.Infrastructure.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.CQRS;

public abstract class QueryHandler<TQuery, TEntity> : RequestHandler<TQuery, object?> where TQuery : Query
{
    protected abstract Task<List<TEntity>> FetchAsync(TQuery query, CancellationToken token);
    protected Task<List<TEntity>> ListAsync(IQueryable<TEntity> queryable, TQuery query, CancellationToken token)
    {
        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            queryable = queryable.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            queryable = queryable.Skip(query.Offset).Take(query.Limit);
        }

        return queryable.ToListAsync<TEntity>(token);
    }

    public override async Task<object?> InvokeAsync(TQuery query, CancellationToken token)
    {
        return await FetchAsync(query, token);
    }
}

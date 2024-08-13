using Commerce.Infrastructure.Dispatcher;

namespace Commerce.Infrastructure.CQRS;

public abstract class QueryHandler<TQuery, TEntity> : RequestHandler<TQuery, object?> where TQuery : Query
{
    protected abstract Task<TEntity?> FindAsync(TQuery query, CancellationToken token);
    protected abstract Task<List<TEntity>> ListAsync(TQuery query, CancellationToken token);

    public override async Task<object?> InvokeAsync(TQuery query, CancellationToken token)
    {
        switch (query.Action)
        {
            case QueryAction.Find: return await FindAsync(query, token);
            case QueryAction.List: return await ListAsync(query, token);
            default: return null;
        }
    }
}

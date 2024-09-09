using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Handlers;

public class ItemQueryHandler : QueryHandler<ItemQuery, Item>
{
    private DataContext context;

    public ItemQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<Item>> FetchAsync(ItemQuery query, CancellationToken token)
    {
        var queryable = context.Items.AsQueryable();

        foreach (var parameter in query.Parameters)
        {
            switch (parameter.Value)
            {
                case int value:
                    queryable = queryable.Filter(parameter.Key, value);
                    break;
                case decimal value:
                    queryable = queryable.Filter(parameter.Key, value);
                    break;
                case string value:
                    queryable = queryable.Filter(parameter.Key, value);
                    break;
            }
        }

        return await ListAsync(queryable, query, token);
    }
}

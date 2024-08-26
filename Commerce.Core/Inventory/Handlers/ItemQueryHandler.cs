using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Identity.Handlers;

public class ItemQueryHandler : QueryHandler<ItemQuery, Item>
{
    private DataContext context;

    public ItemQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Item?> FindAsync(ItemQuery query, CancellationToken token)
    {
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Items.FindAsync(query.Parameters["Id"], token);
        }

        if (query.Parameters.ContainsKey("Code"))
        {
            var code = query.Parameters["Code"] as string;
            return await context.Items.FirstOrDefaultAsync(enterprise => enterprise.Code!.ToLower() == code!.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Item>> ListAsync(ItemQuery query, CancellationToken token)
    {
        var items = context.Items.AsQueryable();
        foreach (var parameter in query.Parameters)
        {
            items = items.Filter(parameter.Key, parameter.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            items = items.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            items = items.Skip(query.Offset).Take(query.Limit);
        }

        return items.ToListAsync(token);
    }
}

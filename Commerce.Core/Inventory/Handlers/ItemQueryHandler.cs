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
        if (query.Id > 0)
        {
            return await context.Items.FindAsync(query.Id, token);
        }

        if (!string.IsNullOrEmpty(query.Code))
        {
            return await context.Items.FirstOrDefaultAsync(item => item.Code.ToLower() == query.Code.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Item>> ListAsync(ItemQuery query, CancellationToken token)
    {
        var items = context.Items.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            items = items.Where(item => item.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Unit))
        {
            items = items.Where(item => item.Unit!.ToLower().Contains(query.Unit.ToLower()));
        }

        if (query.Cost != null)
        {
            items = items.Where(item => item.Cost == query.Cost);
        }

        if (query.Price != null)
        {
            items = items.Where(item => item.Price == query.Price);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            items = items.Sort(query.Sort, query.Reverse);
        }

        return items.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}

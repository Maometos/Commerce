using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Identity.Handlers;

public class BrandQueryHandler : QueryHandler<BrandQuery, Brand>
{
    private DataContext context;

    public BrandQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Brand?> FindAsync(BrandQuery query, CancellationToken token)
    {
        if (query.Id > 0)
        {
            return await context.Brands.FindAsync(query.Id, token);
        }

        return null;
    }

    protected override Task<List<Brand>> ListAsync(BrandQuery query, CancellationToken token)
    {
        var brands = context.Brands.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            brands = context.Brands.Where(user => user.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            brands = brands.Sort(query.Sort, query.Reverse);
        }

        return brands.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}

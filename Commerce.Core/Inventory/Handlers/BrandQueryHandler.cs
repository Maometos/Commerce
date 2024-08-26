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
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Brands.FindAsync(query.Parameters["Id"], token);
        }

        return null;
    }

    protected override Task<List<Brand>> ListAsync(BrandQuery query, CancellationToken token)
    {
        var brands = context.Brands.AsQueryable();
        if (query.Parameters.ContainsKey("Name"))
        {
            brands = brands.Filter("Name", query.Parameters["Name"]);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            brands = brands.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            brands = brands.Skip(query.Offset).Take(query.Limit);
        }

        return brands.ToListAsync(token);
    }
}

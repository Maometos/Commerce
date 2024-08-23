using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common.Handlers;

public class TaxQueryHandler : QueryHandler<TaxQuery, Tax>
{
    private DataContext context;

    public TaxQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Tax?> FindAsync(TaxQuery query, CancellationToken token)
    {
        if (query.Id > 0)
        {
            return await context.Taxes.FindAsync(query.Id, token);
        }

        return null;
    }

    protected override Task<List<Tax>> ListAsync(TaxQuery query, CancellationToken token)
    {
        var enterprises = context.Taxes.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            enterprises = enterprises.Sort(query.Sort, query.Reverse);
        }

        return enterprises.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}

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
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Taxes.FindAsync(query.Parameters["Id"], token);
        }

        return null;
    }

    protected override Task<List<Tax>> ListAsync(TaxQuery query, CancellationToken token)
    {
        var taxes = context.Taxes.AsQueryable();
        if (query.Parameters.ContainsKey("Name"))
        {
            taxes = taxes.Filter("Name", (string)query.Parameters["Name"]);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            taxes = taxes.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            taxes = taxes.Skip(query.Offset).Take(query.Limit);
        }

        return taxes.ToListAsync(token);
    }
}

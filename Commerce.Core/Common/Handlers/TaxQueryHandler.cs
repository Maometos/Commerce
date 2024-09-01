using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common.Handlers;

public class TaxQueryHandler : QueryHandler<TaxGroupQuery, TaxGroup>
{
    private DataContext context;

    public TaxQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<TaxGroup?> FindAsync(TaxGroupQuery query, CancellationToken token)
    {
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.TaxGroups.FindAsync(query.Parameters["Id"], token);
        }

        return null;
    }

    protected override Task<List<TaxGroup>> ListAsync(TaxGroupQuery query, CancellationToken token)
    {
        var groups = context.TaxGroups.AsQueryable();
        if (query.Parameters.ContainsKey("Name"))
        {
            groups = groups.Filter("Name", (string)query.Parameters["Name"]);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            groups = groups.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            groups = groups.Skip(query.Offset).Take(query.Limit);
        }

        return groups.ToListAsync(token);
    }
}

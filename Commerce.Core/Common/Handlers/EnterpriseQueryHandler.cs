using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common.Handlers;

public class EnterpriseQueryHandler : QueryHandler<EnterpriseQuery, Enterprise>
{
    private DataContext context;

    public EnterpriseQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Enterprise?> FindAsync(EnterpriseQuery query, CancellationToken token)
    {
        if (query.Id > 0)
        {
            return await context.Enterprises.FindAsync(query.Id, token);
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            return await context.Enterprises.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == query.Email.ToLower(), token);
        }

        if (!string.IsNullOrEmpty(query.Phone))
        {
            return await context.Enterprises.FirstOrDefaultAsync(enterprise => enterprise.Phone!.ToLower() == query.Phone.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Enterprise>> ListAsync(EnterpriseQuery query, CancellationToken token)
    {
        var enterprises = context.Enterprises.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Email!.ToLower().Contains(query.Email.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Phone))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Phone!.ToLower().Contains(query.Phone.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Locality))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Locality!.ToLower().Contains(query.Locality.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Territory))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Territory!.ToLower().Contains(query.Territory.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Country))
        {
            enterprises = enterprises.Where(enterprise => enterprise.Country!.ToLower().Contains(query.Country.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            enterprises = enterprises.Sort(query.Sort, query.Reverse);
        }

        return enterprises.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}

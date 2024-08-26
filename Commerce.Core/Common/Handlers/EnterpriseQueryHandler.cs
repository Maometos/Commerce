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
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Enterprises.FindAsync(query.Parameters["Id"], token);
        }

        if (query.Parameters.ContainsKey("Email"))
        {
            var email = query.Parameters["Email"] as string;
            return await context.Enterprises.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == email!.ToLower(), token);
        }

        if (query.Parameters.ContainsKey("Phone"))
        {
            var phone = query.Parameters["Phone"] as string;
            return await context.Enterprises.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == phone!.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Enterprise>> ListAsync(EnterpriseQuery query, CancellationToken token)
    {
        var enterprises = context.Enterprises.AsQueryable();

        foreach (var parameter in query.Parameters)
        {
            enterprises = enterprises.Filter(parameter.Key, parameter.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            enterprises = enterprises.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            enterprises = enterprises.Skip(query.Offset).Take(query.Limit);
        }

        return enterprises.ToListAsync(token);
    }
}

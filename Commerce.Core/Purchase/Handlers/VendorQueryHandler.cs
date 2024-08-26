using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Purchase.Handlers;

public class VendorQueryHandler : QueryHandler<VendorQuery, Vendor>
{
    private DataContext context;

    public VendorQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Vendor?> FindAsync(VendorQuery query, CancellationToken token)
    {
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Vendors.FindAsync(query.Parameters["Id"], token);
        }

        if (query.Parameters.ContainsKey("Email"))
        {
            var email = query.Parameters["Email"] as string;
            return await context.Vendors.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == email!.ToLower(), token);
        }

        if (query.Parameters.ContainsKey("Phone"))
        {
            var phone = query.Parameters["Phone"] as string;
            return await context.Vendors.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == phone!.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Vendor>> ListAsync(VendorQuery query, CancellationToken token)
    {
        var vendors = context.Vendors.AsQueryable();
        foreach (var parameter in query.Parameters)
        {
            vendors = vendors.Filter(parameter.Key, parameter.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            vendors = vendors.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            vendors = vendors.Skip(query.Offset).Take(query.Limit);
        }

        return vendors.ToListAsync(token);
    }
}

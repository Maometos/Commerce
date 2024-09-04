using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Purchase.Handlers;

public class SupplierQueryHandler : QueryHandler<SupplierQuery, Supplier>
{
    private DataContext context;

    public SupplierQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Supplier?> FindAsync(SupplierQuery query, CancellationToken token)
    {
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Suppliers.FindAsync(query.Parameters["Id"], token);
        }

        if (query.Parameters.ContainsKey("Email"))
        {
            var email = query.Parameters["Email"] as string;
            return await context.Suppliers.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == email!.ToLower(), token);
        }

        if (query.Parameters.ContainsKey("Phone"))
        {
            var phone = query.Parameters["Phone"] as string;
            return await context.Suppliers.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == phone!.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Supplier>> ListAsync(SupplierQuery query, CancellationToken token)
    {
        var suppliers = context.Suppliers.AsQueryable();
        foreach (var parameter in query.Parameters)
        {
            suppliers = suppliers.Filter(parameter.Key, parameter.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            suppliers = suppliers.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            suppliers = suppliers.Skip(query.Offset).Take(query.Limit);
        }

        return suppliers.ToListAsync(token);
    }
}

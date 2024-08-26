using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Handlers;

public class CustomerQueryHandler : QueryHandler<CustomerQuery, Customer>
{
    private DataContext context;

    public CustomerQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Customer?> FindAsync(CustomerQuery query, CancellationToken token)
    {
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Customers.FindAsync(query.Parameters["Id"], token);
        }

        if (query.Parameters.ContainsKey("Email"))
        {
            var email = query.Parameters["Email"] as string;
            return await context.Customers.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == email!.ToLower(), token);
        }

        if (query.Parameters.ContainsKey("Phone"))
        {
            var phone = query.Parameters["Phone"] as string;
            return await context.Customers.FirstOrDefaultAsync(enterprise => enterprise.Email!.ToLower() == phone!.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Customer>> ListAsync(CustomerQuery query, CancellationToken token)
    {
        var customers = context.Customers.AsQueryable();
        foreach (var parameter in query.Parameters)
        {
            customers = customers.Filter(parameter.Key, parameter.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            customers = customers.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            customers = customers.Skip(query.Offset).Take(query.Limit);
        }

        return customers.ToListAsync(token);
    }
}

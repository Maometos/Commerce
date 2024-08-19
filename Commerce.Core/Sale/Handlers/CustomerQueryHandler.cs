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
        if (query.Id > 0)
        {
            return await context.Customers.FindAsync(query.Id, token);
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            return await context.Customers.FirstOrDefaultAsync(customer => customer.Email!.ToLower() == query.Email.ToLower(), token);
        }

        if (!string.IsNullOrEmpty(query.Phone))
        {
            return await context.Customers.FirstOrDefaultAsync(customer => customer.Phone!.ToLower() == query.Phone.ToLower(), token);
        }

        return null;
    }

    protected override Task<List<Customer>> ListAsync(CustomerQuery query, CancellationToken token)
    {
        var customers = context.Customers.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            customers = customers.Where(customer => customer.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            customers = customers.Where(customer => customer.Email!.ToLower().Contains(query.Email.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Phone))
        {
            customers = customers.Where(customer => customer.Phone!.ToLower().Contains(query.Phone.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Locality))
        {
            customers = customers.Where(customer => customer.Locality!.ToLower().Contains(query.Locality.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Territory))
        {
            customers = customers.Where(customer => customer.Territory!.ToLower().Contains(query.Territory.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Country))
        {
            customers = customers.Where(customer => customer.Country!.ToLower().Contains(query.Country.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            customers = customers.Sort(query.Sort, query.Reverse);
        }

        return customers.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}

using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Handlers;

public class CustomerCommandHandler : CommandHandler<CustomerCommand>
{
    private DataContext context;

    public CustomerCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(CustomerCommand command, CancellationToken token)
    {
        var customer = command.Argument as Customer;
        if (customer == null)
        {
            return 0;
        }

        context.Customers.Add(customer);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(CustomerCommand command, CancellationToken token)
    {
        var customer = command.Argument as Customer;
        if (customer == null)
        {
            return 0;
        }

        var entity = await context.Customers.FindAsync(customer.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Customers.Update(customer);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(CustomerCommand command, CancellationToken token)
    {
        var customer = await context.Customers.FindAsync(command.Argument, token);
        if (customer == null)
        {
            return 0;
        }

        context.Customers.Remove(customer);
        return await context.SaveChangesAsync(token);
    }
}

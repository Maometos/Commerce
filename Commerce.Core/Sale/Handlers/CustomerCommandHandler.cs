using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;

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
        var item = new Customer();
        item.Name = command.Name!;
        item.Email = command.Email!;
        item.Phone = command.Phone!;
        item.Address = command.Address!;
        item.Locality = command.Locality!;
        item.Territory = command.Territory!;
        item.Postcode = command.Postcode!;
        item.Country = command.Country!;

        context.Customers.Add(item);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(CustomerCommand command, CancellationToken token)
    {
        var item = await context.Customers.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        item.Name = command.Name ?? item.Name;
        item.Email = command.Email ?? item.Email;
        item.Phone = command.Phone ?? item.Phone;
        item.Address = command.Address ?? item.Address;
        item.Locality = command.Locality ?? item.Locality;
        item.Territory = command.Territory ?? item.Territory;
        item.Postcode = command.Postcode ?? item.Postcode;
        item.Country = command.Country ?? item.Country;

        context.Customers.Update(item);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(CustomerCommand command, CancellationToken token)
    {
        var item = await context.Customers.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        context.Customers.Remove(item);
        return await context.SaveChangesAsync(token);
    }
}

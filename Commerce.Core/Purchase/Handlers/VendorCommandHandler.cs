using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Requests;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Purchase.Handlers;

public class VendorCommandHandler : CommandHandler<VendorCommand>
{
    private DataContext context;

    public VendorCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(VendorCommand command, CancellationToken token)
    {
        var item = new Vendor();
        item.Name = command.Name!;
        item.Email = command.Email!;
        item.Phone = command.Phone!;
        item.Address = command.Address!;
        item.Locality = command.Locality!;
        item.Territory = command.Territory!;
        item.Postcode = command.Postcode!;
        item.Country = command.Country!;

        context.Vendors.Add(item);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(VendorCommand command, CancellationToken token)
    {
        var item = await context.Vendors.FindAsync(command.Id, token);
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

        context.Vendors.Update(item);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(VendorCommand command, CancellationToken token)
    {
        var item = await context.Vendors.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        context.Vendors.Remove(item);
        return await context.SaveChangesAsync(token);
    }
}

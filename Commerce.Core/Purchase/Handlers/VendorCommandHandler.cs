using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

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
        var vendor = command.Argument as Vendor;
        if (vendor == null)
        {
            return 0;
        }

        context.Vendors.Add(vendor);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(VendorCommand command, CancellationToken token)
    {
        var vendor = command.Argument as Vendor;
        if (vendor == null)
        {
            return 0;
        }

        var entity = await context.Vendors.FindAsync(vendor.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Vendors.Update(vendor);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(VendorCommand command, CancellationToken token)
    {
        var vendor = await context.Vendors.FindAsync(command.Argument, token);
        if (vendor == null)
        {
            return 0;
        }

        context.Vendors.Remove(vendor);
        return await context.SaveChangesAsync(token);
    }
}

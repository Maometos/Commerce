using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Purchase.Handlers;

public class SupplierCommandHandler : CommandHandler<SupplierCommand>
{
    private DataContext context;

    public SupplierCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(SupplierCommand command, CancellationToken token)
    {
        var supplier = command.Argument as Supplier;
        if (supplier == null)
        {
            return 0;
        }

        context.Suppliers.Add(supplier);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(SupplierCommand command, CancellationToken token)
    {
        var supplier = command.Argument as Supplier;
        if (supplier == null)
        {
            return 0;
        }

        var entity = await context.Suppliers.FindAsync(supplier.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Suppliers.Update(supplier);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(SupplierCommand command, CancellationToken token)
    {
        var supplier = await context.Suppliers.FindAsync(command.Argument, token);
        if (supplier == null)
        {
            return 0;
        }

        context.Suppliers.Remove(supplier);
        return await context.SaveChangesAsync(token);
    }
}

using Commerce.Core.Common;
using Commerce.Core.Invoice.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Invoice.Commands;

public class PurchaseInvoiceCommandHandler : CommandHandler<PurchaseInvoiceCommand>
{
    private DataContext context;

    public PurchaseInvoiceCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(PurchaseInvoiceCommand command, CancellationToken token)
    {
        var bill = command.Argument as PurchaseInvoice;
        if (bill == null)
        {
            return 0;
        }

        context.PurchaseInvoices.Add(bill);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(PurchaseInvoiceCommand command, CancellationToken token)
    {
        var bill = command.Argument as PurchaseInvoice;
        if (bill == null)
        {
            return 0;
        }

        var entity = await context.PurchaseInvoices.FindAsync(bill.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.PurchaseInvoices.Update(bill);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(PurchaseInvoiceCommand command, CancellationToken token)
    {
        var bill = await context.PurchaseInvoices.FindAsync(command.Argument, token);
        if (bill == null)
        {
            return 0;
        }

        context.PurchaseInvoices.Remove(bill);
        return await context.SaveChangesAsync(token);
    }
}

using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Handlers;

public class SaleInvoiceCommandHandler : CommandHandler<SaleInvoiceCommand>
{
    private DataContext context;

    public SaleInvoiceCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(SaleInvoiceCommand command, CancellationToken token)
    {
        var invoice = command.Argument as SaleInvoice;
        if (invoice == null)
        {
            return 0;
        }

        context.SaleInvoices.Add(invoice);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(SaleInvoiceCommand command, CancellationToken token)
    {
        var invoice = command.Argument as SaleInvoice;
        if (invoice == null)
        {
            return 0;
        }

        var entity = await context.SaleInvoices.FindAsync(invoice.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.SaleInvoices.Update(invoice);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(SaleInvoiceCommand command, CancellationToken token)
    {
        var invoice = await context.SaleInvoices.FindAsync(command.Argument, token);
        if (invoice == null)
        {
            return 0;
        }

        context.SaleInvoices.Remove(invoice);
        return await context.SaveChangesAsync(token);
    }
}

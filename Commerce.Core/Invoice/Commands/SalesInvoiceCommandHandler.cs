using Commerce.Core.Common;
using Commerce.Core.Invoice.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Invoice.Commands;

public class SalesInvoiceCommandHandler : CommandHandler<SalesInvoiceCommand>
{
    private DataContext context;

    public SalesInvoiceCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(SalesInvoiceCommand command, CancellationToken token)
    {
        var invoice = command.Argument as SalesInvoice;
        if (invoice == null)
        {
            return 0;
        }

        context.SaleInvoices.Add(invoice);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(SalesInvoiceCommand command, CancellationToken token)
    {
        var invoice = command.Argument as SalesInvoice;
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

    protected override async Task<int> DeleteAsync(SalesInvoiceCommand command, CancellationToken token)
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

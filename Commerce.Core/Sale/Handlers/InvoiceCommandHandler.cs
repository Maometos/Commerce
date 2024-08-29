using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Handlers;

public class InvoiceCommandHandler : CommandHandler<InvoiceCommand>
{
    private DataContext context;

    public InvoiceCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(InvoiceCommand command, CancellationToken token)
    {
        var invoice = command.Argument as Invoice;
        if (invoice == null)
        {
            return 0;
        }

        context.Invoices.Add(invoice);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(InvoiceCommand command, CancellationToken token)
    {
        var invoice = command.Argument as Invoice;
        if (invoice == null)
        {
            return 0;
        }

        var entity = await context.Invoices.FindAsync(invoice.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Invoices.Update(invoice);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(InvoiceCommand command, CancellationToken token)
    {
        var invoice = await context.Invoices.FindAsync(command.Argument, token);
        if (invoice == null)
        {
            return 0;
        }

        context.Invoices.Remove(invoice);
        return await context.SaveChangesAsync(token);
    }
}

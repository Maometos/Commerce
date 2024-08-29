using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Purchase.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Handlers;

public class BillCommandHandler : CommandHandler<BillCommand>
{
    private DataContext context;

    public BillCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(BillCommand command, CancellationToken token)
    {
        var bill = command.Argument as Bill;
        if (bill == null)
        {
            return 0;
        }

        context.Bills.Add(bill);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(BillCommand command, CancellationToken token)
    {
        var bill = command.Argument as Bill;
        if (bill == null)
        {
            return 0;
        }

        var entity = await context.Bills.FindAsync(bill.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Bills.Update(bill);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(BillCommand command, CancellationToken token)
    {
        var bill = await context.Bills.FindAsync(command.Argument, token);
        if (bill == null)
        {
            return 0;
        }

        context.Bills.Remove(bill);
        return await context.SaveChangesAsync(token);
    }
}

using Commerce.Core.Common;
using Commerce.Core.Sale.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Sale.Commands;

public class CreditMemoCommandHandler : CommandHandler<CreditMemoCommand>
{
    private DataContext context;

    public CreditMemoCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(CreditMemoCommand command, CancellationToken token)
    {
        var creditMemo = command.Argument as CreditMemo;
        if (creditMemo == null)
        {
            return 0;
        }

        context.CreditMemos.Add(creditMemo);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(CreditMemoCommand command, CancellationToken token)
    {
        var creditMemo = command.Argument as CreditMemo;
        if (creditMemo == null)
        {
            return 0;
        }

        var entity = await context.CreditMemos.FindAsync(creditMemo.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.CreditMemos.Update(creditMemo);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(CreditMemoCommand command, CancellationToken token)
    {
        var creditMemo = await context.CreditMemos.FindAsync(command.Argument, token);
        if (creditMemo == null)
        {
            return 0;
        }

        context.CreditMemos.Remove(creditMemo);
        return await context.SaveChangesAsync(token);
    }
}

using Commerce.Core.Common;
using Commerce.Core.Memo.Commands;
using Commerce.Core.Memo.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Memo.Handlers;

public class DebitMemoCommandHandler : CommandHandler<DebitMemoCommand>
{
    private DataContext context;

    public DebitMemoCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(DebitMemoCommand command, CancellationToken token)
    {
        var debitMemo = command.Argument as DebitMemo;
        if (debitMemo == null)
        {
            return 0;
        }

        context.DebitMemos.Add(debitMemo);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(DebitMemoCommand command, CancellationToken token)
    {
        var debitMemo = command.Argument as DebitMemo;
        if (debitMemo == null)
        {
            return 0;
        }

        var entity = await context.DebitMemos.FindAsync(debitMemo.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.DebitMemos.Update(debitMemo);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(DebitMemoCommand command, CancellationToken token)
    {
        var debitMemo = await context.DebitMemos.FindAsync(command.Argument, token);
        if (debitMemo == null)
        {
            return 0;
        }

        context.DebitMemos.Remove(debitMemo);
        return await context.SaveChangesAsync(token);
    }
}

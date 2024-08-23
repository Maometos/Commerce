using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common.Handlers;

public class TaxCommandHandler : CommandHandler<TaxCommand>
{
    private DataContext context;

    public TaxCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(TaxCommand command, CancellationToken token)
    {
        var enterprise = command.Argument as Tax;
        if (enterprise == null)
        {
            return 0;
        }

        context.Taxes.Add(enterprise);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(TaxCommand command, CancellationToken token)
    {
        var enterprise = command.Argument as Tax;
        if (enterprise == null)
        {
            return 0;
        }

        var entity = await context.Taxes.FindAsync(enterprise.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Taxes.Update(enterprise);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(TaxCommand command, CancellationToken token)
    {
        var enterprise = await context.Taxes.FindAsync(command.Argument, token);
        if (enterprise == null)
        {
            return 0;
        }

        context.Taxes.Remove(enterprise);
        return await context.SaveChangesAsync(token);
    }
}

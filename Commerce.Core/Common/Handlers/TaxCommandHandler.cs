using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Common.Handlers;

public class TaxCommandHandler : CommandHandler<TaxGroupCommand>
{
    private DataContext context;

    public TaxCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(TaxGroupCommand command, CancellationToken token)
    {
        var group = command.Argument as TaxGroup;
        if (group == null)
        {
            return 0;
        }

        context.TaxGroups.Add(group);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(TaxGroupCommand command, CancellationToken token)
    {
        var group = command.Argument as TaxGroup;
        if (group == null)
        {
            return 0;
        }

        var entity = await context.TaxGroups.FindAsync(group.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.TaxGroups.Update(group);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(TaxGroupCommand command, CancellationToken token)
    {
        var group = await context.TaxGroups.FindAsync(command.Argument, token);
        if (group == null)
        {
            return 0;
        }

        context.TaxGroups.Remove(group);
        return await context.SaveChangesAsync(token);
    }
}

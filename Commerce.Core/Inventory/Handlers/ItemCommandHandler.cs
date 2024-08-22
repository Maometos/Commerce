using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Identity.Handlers;

public class ItemCommandHandler : CommandHandler<ItemCommand>
{
    private DataContext context;

    public ItemCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(ItemCommand command, CancellationToken token)
    {
        var item = command.Argument as Item;
        if (item == null)
        {
            return 0;
        }

        context.Items.Add(item);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(ItemCommand command, CancellationToken token)
    {
        var item = command.Argument as Item;
        if (item == null)
        {
            return 0;
        }

        var entity = await context.Items.FindAsync(item.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Items.Update(item);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(ItemCommand command, CancellationToken token)
    {
        var item = await context.Items.FindAsync(command.Argument, token);
        if (item == null)
        {
            return 0;
        }

        context.Items.Remove(item);
        return await context.SaveChangesAsync(token);
    }
}

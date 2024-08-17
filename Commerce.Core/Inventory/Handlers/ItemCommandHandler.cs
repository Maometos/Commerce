using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;

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
        var item = new Item();
        item.Code = command.Code!;
        item.Name = command.Name!;
        item.Unit = command.Unit!;
        item.Cost = command.Cost ?? 0;
        item.Price = command.Price ?? 0;
        item.Description = command.Description!;

        context.Items.Add(item);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(ItemCommand command, CancellationToken token)
    {
        var item = await context.Items.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        item.Code = command.Code ?? item.Code;
        item.Name = command.Name ?? item.Name;
        item.Unit = command.Unit ?? item.Unit;
        item.Cost = command.Cost ?? item.Cost;
        item.Price = command.Price ?? item.Price;
        item.Description = command.Description ?? item.Description;

        context.Items.Update(item);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(ItemCommand command, CancellationToken token)
    {
        var item = await context.Items.FindAsync(command.Id, token);
        if (item == null)
        {
            return 0;
        }

        context.Items.Remove(item);
        return await context.SaveChangesAsync(token);
    }
}

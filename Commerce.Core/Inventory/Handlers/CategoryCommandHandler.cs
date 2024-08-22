using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Identity.Handlers;

public class CategoryCommandHandler : CommandHandler<CategoryCommand>
{
    private DataContext context;

    public CategoryCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(CategoryCommand command, CancellationToken token)
    {
        var category = command.Argument as Category;
        if (category == null)
        {
            return 0;
        }

        context.Categories.Add(category);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(CategoryCommand command, CancellationToken token)
    {
        var category = command.Argument as Category;
        if (category == null)
        {
            return 0;
        }

        var entity = await context.Categories.FindAsync(category.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Categories.Update(category);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(CategoryCommand command, CancellationToken token)
    {
        var category = await context.Categories.FindAsync(command.Argument, token);
        if (category == null)
        {
            return 0;
        }

        context.Categories.Remove(category);
        return await context.SaveChangesAsync(token);
    }
}

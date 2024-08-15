using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;

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
        var category = new Category();
        category.Name = command.Name!;
        category.Description = command.Description;

        context.Categories.Add(category);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(CategoryCommand command, CancellationToken token)
    {
        var category = await context.Categories.FindAsync(command.Id, token);
        if (category == null)
        {
            return 0;
        }

        category.Name = command.Name!;
        category.Description = command.Description;

        context.Categories.Update(category);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(CategoryCommand command, CancellationToken token)
    {
        var category = await context.Categories.FindAsync(command.Id, token);
        if (category == null)
        {
            return 0;
        }

        context.Categories.Remove(category);
        return await context.SaveChangesAsync(token);
    }
}

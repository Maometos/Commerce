using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Identity.Handlers;

public class BrandCommandHandler : CommandHandler<BrandCommand>
{
    private DataContext context;

    public BrandCommandHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<int> CreateAsync(BrandCommand command, CancellationToken token)
    {
        var brand = command.Argument as Brand;
        if (brand == null)
        {
            return 0;
        }

        context.Brands.Add(brand);
        return await context.SaveChangesAsync();
    }

    protected override async Task<int> UpdateAsync(BrandCommand command, CancellationToken token)
    {
        var brand = command.Argument as Brand;
        if (brand == null)
        {
            return 0;
        }

        var entity = await context.Brands.FindAsync(brand.Id, token);
        if (entity == null)
        {
            return 0;
        }

        context.Entry(entity).State = EntityState.Detached;

        context.Brands.Update(brand);
        return await context.SaveChangesAsync(token);
    }

    protected override async Task<int> DeleteAsync(BrandCommand command, CancellationToken token)
    {
        var brand = await context.Brands.FindAsync(command.Argument, token);
        if (brand == null)
        {
            return 0;
        }

        context.Brands.Remove(brand);
        return await context.SaveChangesAsync(token);
    }
}

using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Core.Identity.Handlers;

public class CategoryQueryHandler : QueryHandler<CategoryQuery, Category>
{
    private DataContext context;

    public CategoryQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<Category?> FindAsync(CategoryQuery query, CancellationToken token)
    {
        if (query.Id > 0)
        {
            return await context.Categories.FindAsync(query.Id, token);
        }

        return null;
    }

    protected override Task<List<Category>> ListAsync(CategoryQuery query, CancellationToken token)
    {
        var categories = context.Categories.AsQueryable();
        if (!string.IsNullOrEmpty(query.Name))
        {
            categories = context.Categories.Where(user => user.Name.ToLower().Contains(query.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            categories = categories.Sort(query.Sort, query.Reverse);
        }

        return categories.Paginate(query.Page, query.Limit).ToListAsync(token);
    }
}

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
        if (query.Parameters.ContainsKey("Id"))
        {
            return await context.Categories.FindAsync(query.Parameters["Id"], token);
        }

        return null;
    }

    protected override Task<List<Category>> ListAsync(CategoryQuery query, CancellationToken token)
    {
        var categories = context.Categories.AsQueryable();
        if (query.Parameters.ContainsKey("Name"))
        {
            categories = categories.Filter("Name", query.Parameters["Name"]);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var reverse = false;
            var sort = query.Sort.Trim('-');
            if (sort.Length != query.Sort.Length)
            {
                reverse = true;
            }
            categories = categories.Sort(sort, reverse);
        }

        if (query.Offset > 0 && query.Limit > 0)
        {
            categories = categories.Skip(query.Offset).Take(query.Limit);
        }

        return categories.ToListAsync(token);
    }
}

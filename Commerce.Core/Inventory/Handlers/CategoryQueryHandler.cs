using Commerce.Core.Common;
using Commerce.Core.Identity.Requests;
using Commerce.Core.Inventory.Entities;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Handlers;

public class CategoryQueryHandler : QueryHandler<CategoryQuery, Category>
{
    private DataContext context;

    public CategoryQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<Category>> FetchAsync(CategoryQuery query, CancellationToken token)
    {
        var queryable = context.Categories.AsQueryable();
        if (query.Parameters.ContainsKey("Id"))
        {
            queryable = queryable.Filter("Id", query.Parameters["Id"]);
        }

        if (query.Parameters.ContainsKey("Name"))
        {
            queryable = queryable.Filter("Name", query.Parameters["Name"]);
        }

        return await ListAsync(queryable, query, token);
    }
}

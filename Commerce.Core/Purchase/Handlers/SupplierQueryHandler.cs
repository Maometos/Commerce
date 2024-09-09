using Commerce.Core.Common;
using Commerce.Core.Purchase.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Purchase.Handlers;

public class SupplierQueryHandler : QueryHandler<SupplierQuery, Supplier>
{
    private DataContext context;

    public SupplierQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<Supplier>> FetchAsync(SupplierQuery query, CancellationToken token)
    {
        var queryable = context.Suppliers.AsQueryable();

        foreach (var parameter in query.Parameters)
        {
            switch (parameter.Value)
            {
                case int value:
                    queryable = queryable.Filter(parameter.Key, value);
                    break;
                case string value:
                    queryable = queryable.Filter(parameter.Key, value);
                    break;
            }
        }

        return await ListAsync(queryable, query, token);
    }
}

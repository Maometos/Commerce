using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Handlers;

public class TaxQueryHandler : QueryHandler<TaxGroupQuery, TaxGroup>
{
    private DataContext context;

    public TaxQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<TaxGroup>> FetchAsync(TaxGroupQuery query, CancellationToken token)
    {
        var queryable = context.TaxGroups.AsQueryable();
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

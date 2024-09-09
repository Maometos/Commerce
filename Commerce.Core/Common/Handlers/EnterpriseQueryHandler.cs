using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Requests;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Handlers;

public class EnterpriseQueryHandler : QueryHandler<EnterpriseQuery, Enterprise>
{
    private DataContext context;

    public EnterpriseQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<Enterprise>> FetchAsync(EnterpriseQuery query, CancellationToken token)
    {
        var queryable = context.Enterprises.AsQueryable();

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

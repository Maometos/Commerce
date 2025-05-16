using Commerce.Core.Common;
using Commerce.Core.Contacts.Entities;
using Commerce.Core.Contacts.Queries;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Contacts.Handlers;

public class CustomerQueryHandler : QueryHandler<CustomerQuery, Customer>
{
    private DataContext context;

    public CustomerQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<Customer>> FetchAsync(CustomerQuery query, CancellationToken token)
    {
        var queryable = context.Customers.AsQueryable();

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

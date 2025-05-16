using Commerce.Core.Common;
using Commerce.Core.Common.Values;
using Commerce.Core.Invoice.Entities;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Invoice.Queries;

public class SalesInvoiceQueryHandler : QueryHandler<SalesInvoiceQuery, SalesInvoice>
{
    private DataContext context;

    public SalesInvoiceQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<SalesInvoice>> FetchAsync(SalesInvoiceQuery query, CancellationToken token)
    {
        var queryable = context.SaleInvoices.AsQueryable();

        if (query.Parameters.ContainsKey("Id"))
        {
            queryable = queryable.Where(e => e.Id == (int)query.Parameters["Id"]);
        }

        if (query.Parameters.ContainsKey("Reference"))
        {
            queryable = queryable.Where(e => e.Reference == (string)query.Parameters["Reference"]);
        }

        if (query.Parameters.ContainsKey("Total"))
        {
            queryable = queryable.Where(e => e.Total == (decimal)query.Parameters["Total"]);
        }

        if (query.Parameters.ContainsKey("Date"))
        {
            var date = (DateTime)query.Parameters["Date"];
            queryable = queryable.Where(e => e.Date.ToString("d") == date.ToString("d"));
        }

        if (query.Parameters.ContainsKey("Status"))
        {
            queryable = queryable.Where(e => e.PaymentStatus == (PaymentStatus)query.Parameters["Status"]);
        }

        return await ListAsync(queryable, query, token);
    }
}

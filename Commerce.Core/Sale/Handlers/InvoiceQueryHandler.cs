﻿using Commerce.Core.Common;
using Commerce.Core.Common.Values;
using Commerce.Core.Sale.Entities;
using Commerce.Core.Sale.Requests;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Sale.Handlers;

public class InvoiceQueryHandler : QueryHandler<InvoiceQuery, Invoice>
{
    private DataContext context;

    public InvoiceQueryHandler(DataContext context)
    {
        this.context = context;
    }

    protected override async Task<List<Invoice>> FetchAsync(InvoiceQuery query, CancellationToken token)
    {
        var queryable = context.Invoices.AsQueryable();

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

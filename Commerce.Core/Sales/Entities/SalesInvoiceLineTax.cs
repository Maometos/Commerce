using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sales.Entities;

public class SalesInvoiceLineTax : TransactionLineTax
{
    public int SaleInvoiceLineId { get; set; }

    public SalesInvoiceLine Line { get; set; } = null!;
}

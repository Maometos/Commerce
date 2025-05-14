using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sales.Entities;

public class SalesInvoiceLine : TransactionLine<SalesInvoiceLineTax>
{
    public int SaleInvoiceId { get; set; }
    public SalesInvoice Invoice { get; set; } = null!;
}

using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class SaleInvoiceLine : TransactionLine<SaleInvoiceLineTax>
{
    public int SaleInvoiceId { get; set; }
    public SaleInvoice Invoice { get; set; } = null!;
}

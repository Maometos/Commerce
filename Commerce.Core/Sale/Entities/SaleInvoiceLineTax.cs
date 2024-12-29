using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class SaleInvoiceLineTax : TransactionLineTax
{
    public int SaleInvoiceLineId { get; set; }

    public SaleInvoiceLine Line { get; set; } = null!;
}

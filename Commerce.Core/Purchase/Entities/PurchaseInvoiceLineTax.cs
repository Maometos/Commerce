using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoiceLineTax : TransactionLineTax
{
    public int PurchaseInvoiceLineId { get; set; }

    public PurchaseInvoiceLine Line { get; set; } = null!;
}

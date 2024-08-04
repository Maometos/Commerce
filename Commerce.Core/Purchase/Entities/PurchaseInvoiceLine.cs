using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoiceLine : TransactionLine
{
    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
}

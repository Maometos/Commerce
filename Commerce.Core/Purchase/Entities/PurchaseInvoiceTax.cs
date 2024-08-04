using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoiceTax : TransactionTax
{
    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
}

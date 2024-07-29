using Invoice.Core.Common.Abstractions;

namespace Invoice.Core.Purchase.Entities;

public class PurchaseInvoiceTax : TransactionTax
{
    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
}

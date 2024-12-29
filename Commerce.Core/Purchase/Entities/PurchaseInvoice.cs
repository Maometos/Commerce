using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoice : Statement<PurchaseInvoiceLine, PurchaseInvoiceLineTax, PaymentAdvice>
{
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
}

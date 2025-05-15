using Commerce.Core.Common.Abstractions;
using Commerce.Core.Contacts.Entities;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoice : Invoice<PurchaseInvoiceLine, PurchaseInvoiceLineTax, PaymentAdvice>
{
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
}

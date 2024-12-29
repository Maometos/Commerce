using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoiceLine : LineItem<PurchaseInvoiceLineTax>
{
    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice Invoice { get; set; } = null!;
}

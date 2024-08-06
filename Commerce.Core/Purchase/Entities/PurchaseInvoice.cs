using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoice : Invoice
{
    public int VendorId { get; set; }

    public Vendor Vendor { get; set; } = null!;
    public List<PurchaseInvoiceLine> Lines { get; } = [];
    public List<PurchaseInvoiceTax> Taxes { get; } = [];
    public List<PurchasePayment> Payments { get; } = [];
}

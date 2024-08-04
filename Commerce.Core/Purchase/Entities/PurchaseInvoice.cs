using Commerce.Core.Common.Abstractions;
using Commerce.Core.Common.Values;

namespace Commerce.Core.Purchase.Entities;

public class PurchaseInvoice : Transaction
{
    public int VendorId { get; set; }
    public DateTime DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public InvoiceStatus Status { get; set; }

    public Vendor Vendor { get; set; } = null!;
    public List<PurchaseInvoiceLine> Lines { get; } = [];
    public List<PurchaseInvoiceTax> Taxes { get; } = [];
}

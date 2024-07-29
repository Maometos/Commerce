using Invoice.Core.Common.Abstractions;
using Invoice.Core.Common.Values;

namespace Invoice.Core.Purchase.Entities;

public class PurchaseInvoice : Transaction
{
    public int VendorId { get; set; }
    public string Number { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status { get; set; }

    public Vendor Vendor { get; set; } = null!;
}

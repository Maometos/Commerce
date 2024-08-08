using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class Bill : Statement
{
    public int VendorId { get; set; }

    public Vendor Vendor { get; set; } = null!;
    public List<BillLine> Lines { get; } = [];
    public List<BillTax> Taxes { get; } = [];
    public List<PurchasePayment> Payments { get; } = [];
}

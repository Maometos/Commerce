using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class Bill : Statement<BillLine, BillLineTax>
{
    public int VendorId { get; set; }

    public Vendor Vendor { get; set; } = null!;
    public List<PurchasePayment> Payments { get; } = [];
}

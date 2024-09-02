using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class DebitNote : Adjustment<DebitLine, DebitLineTax>
{
    public int VendorId { get; set; }

    public Vendor Vendor { get; set; } = null!;
    public List<PurchaseRefund> Refunds { get; } = [];
}

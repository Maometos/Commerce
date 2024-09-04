using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class DebitNote : Adjustment<DebitLine, DebitLineTax>
{
    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;
    public List<PurchaseRefund> Refunds { get; } = [];
}

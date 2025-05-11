using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class DebitMemo : Memo<DebitLine, DebitLineTax>
{
    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;
    public List<RefundReceipt> Refunds { get; } = [];
}

using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class RefundReceipt : Payment
{
    public int DebitMemoId { get; set; }

    public DebitMemo Memo { get; set; } = null!;
}

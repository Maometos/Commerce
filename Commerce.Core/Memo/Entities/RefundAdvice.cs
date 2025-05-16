using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Memo.Entities;

public class RefundAdvice : Payment
{
    public int CreditMemoId { get; set; }

    public CreditMemo Memo { get; set; } = null!;
}

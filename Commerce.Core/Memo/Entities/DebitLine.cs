using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Memo.Entities;

public class DebitLine : TransactionLine<DebitLineTax>
{
    public int DebitMemoId { get; set; }
    public DebitMemo Memo { get; set; } = null!;
}

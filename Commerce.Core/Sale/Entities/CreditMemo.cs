using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditMemo : Memo<CreditLine, CreditLineTax>
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<RefundAdvice> Refunds { get; } = [];
}

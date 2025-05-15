using Commerce.Core.Common.Abstractions;
using Commerce.Core.Contacts.Entities;

namespace Commerce.Core.Sales.Entities;

public class CreditMemo : Memo<CreditLine, CreditLineTax>
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<RefundAdvice> Refunds { get; } = [];
}

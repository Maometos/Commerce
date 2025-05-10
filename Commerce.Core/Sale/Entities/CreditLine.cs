using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditLine : TransactionLine<CreditLineTax>
{
    public int CreditMemoId { get; set; }
    public CreditMemo Memo { get; set; } = null!;
}

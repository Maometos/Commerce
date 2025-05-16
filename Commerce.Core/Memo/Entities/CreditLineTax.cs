using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Memo.Entities;

public class CreditLineTax : TransactionLineTax
{
    public int CreditLineId { get; set; }

    public CreditLine Line { get; set; } = null!;
}

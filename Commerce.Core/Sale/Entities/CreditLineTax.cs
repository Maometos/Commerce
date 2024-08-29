using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditLineTax : TransactionLineTax
{
    public int CreditLineId { get; set; }

    public CreditLine Line { get; set; } = null!;
}

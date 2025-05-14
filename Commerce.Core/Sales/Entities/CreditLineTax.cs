using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sales.Entities;

public class CreditLineTax : TransactionLineTax
{
    public int CreditLineId { get; set; }

    public CreditLine Line { get; set; } = null!;
}

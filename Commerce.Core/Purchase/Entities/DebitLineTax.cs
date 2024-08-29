using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class DebitLineTax : TransactionLineTax
{
    public int DebitLineId { get; set; }

    public DebitLine Line { get; set; } = null!;
}

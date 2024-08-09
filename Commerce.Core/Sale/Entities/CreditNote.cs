using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditNote : Adjustment
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
}

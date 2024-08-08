using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class BillTax : TransactionTax
{
    public int BillId { get; set; }

    public Bill Bill { get; set; } = null!;
}

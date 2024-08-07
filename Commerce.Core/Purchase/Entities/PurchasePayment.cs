using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class PurchasePayment : Payment
{
    public int BillId { get; set; }

    public Bill Bill { get; set; } = null!;
}

using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class Bill : Statement<BillLine, BillLineTax, PaymentAdvice>
{
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
}

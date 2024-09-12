using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class Bill : Statement<BillLine, BillLineTax>
{
    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;
    public List<PaymentAdvice> Payments { get; } = [];
}

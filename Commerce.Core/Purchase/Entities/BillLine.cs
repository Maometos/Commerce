using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class BillLine : LineItem
{
    public int BillId { get; set; }

    public Bill Bill { get; set; } = null!;
    public List<BillLineTax> Taxes { get; set; } = []!;
}

using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class BillLineTax : LineTax
{
    public int BillLineId { get; set; }

    public BillLine Line { get; set; } = null!;
}

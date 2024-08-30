using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Adjustment : Document
{
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public AdjustmentStatus Status { get; set; }
}

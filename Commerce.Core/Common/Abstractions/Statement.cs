using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Statement : Document
{
    public DateTime DueDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public StatementStatus Status { get; set; }
}

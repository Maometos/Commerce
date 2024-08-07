using Commerce.Core.Common.Values;

namespace Commerce.Core.Common.Abstractions;

public abstract class Statement : Transaction
{
    public DateTime DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public InvoiceStatus Status { get; set; }
}

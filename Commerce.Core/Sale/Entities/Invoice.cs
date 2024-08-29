using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class Invoice : Statement
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<InvoiceLine> Lines { get; } = [];
    public List<SalePayment> Payments { get; } = [];
}

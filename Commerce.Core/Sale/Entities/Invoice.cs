using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class Invoice : Statement<InvoiceLine, InvoiceLineTax>
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<SalePayment> Payments { get; } = [];
}

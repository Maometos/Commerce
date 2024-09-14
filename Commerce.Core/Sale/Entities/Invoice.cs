using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class Invoice : Statement<InvoiceLine, InvoiceLineTax, PaymentReceipt>
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}

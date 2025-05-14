using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sales.Entities;

public class SalesInvoice : Invoice<SalesInvoiceLine, SalesInvoiceLineTax, PaymentReceipt>
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}

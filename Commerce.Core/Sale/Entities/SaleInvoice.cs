using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class SaleInvoice : Invoice<SaleInvoiceLine, SaleInvoiceLineTax, PaymentReceipt>
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}

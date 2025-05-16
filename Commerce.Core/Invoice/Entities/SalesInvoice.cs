using Commerce.Core.Common.Abstractions;
using Commerce.Core.Contacts.Entities;

namespace Commerce.Core.Invoice.Entities;

public class SalesInvoice : Invoice<SalesInvoiceLine, SalesInvoiceLineTax, PaymentReceipt>
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}

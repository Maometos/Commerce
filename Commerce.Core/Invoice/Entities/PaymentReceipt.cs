using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Invoice.Entities;

public class PaymentReceipt : Payment
{
    public int SaleInvoiceId { get; set; }

    public SalesInvoice Invoice { get; set; } = null!;
}

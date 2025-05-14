using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sales.Entities;

public class PaymentReceipt : Payment
{
    public int SaleInvoiceId { get; set; }

    public SalesInvoice Invoice { get; set; } = null!;
}

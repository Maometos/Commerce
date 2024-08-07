using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class SalePayment : Payment
{
    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;
}

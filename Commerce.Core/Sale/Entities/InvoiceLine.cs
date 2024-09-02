using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class InvoiceLine : LineItem<InvoiceLineTax>
{
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
}

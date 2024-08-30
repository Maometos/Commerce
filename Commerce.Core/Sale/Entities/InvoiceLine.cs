using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class InvoiceLine : LineItem
{
    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;
    public List<InvoiceLineTax> Taxes { get; set; } = []!;
}

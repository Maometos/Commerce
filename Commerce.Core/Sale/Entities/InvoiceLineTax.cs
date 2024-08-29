using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class InvoiceLineTax : TransactionLineTax
{
    public int InvoiceLineId { get; set; }

    public InvoiceLine Line { get; set; } = null!;
}

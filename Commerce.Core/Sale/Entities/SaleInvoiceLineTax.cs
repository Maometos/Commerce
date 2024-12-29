using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class SaleInvoiceLineTax : LineTax
{
    public int SaleInvoiceLineId { get; set; }

    public SaleInvoiceLine Line { get; set; } = null!;
}

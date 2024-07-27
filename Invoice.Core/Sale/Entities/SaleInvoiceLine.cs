using Invoice.Core.Common.Abstractions;

namespace Invoice.Core.Sale.Entities;

public class SaleInvoiceLine : TransactionLine
{
    public int SaleInvoiceId { get; set; }

    public SaleInvoice SaleInvoice { get; set; } = null!;
}

using Invoice.Core.Common.Abstractions;

namespace Invoice.Core.Sale.Entities;

public class SaleInvoiceTax : TransactionTax
{
    public int SaleInvoiceId { get; set; }

    public SaleInvoice SaleInvoice { get; set; } = null!;
}

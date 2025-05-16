using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Invoice.Entities;

public class PurchaseInvoiceLine : TransactionLine<PurchaseInvoiceLineTax>
{
    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice Invoice { get; set; } = null!;
}

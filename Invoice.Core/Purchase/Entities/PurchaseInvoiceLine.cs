using Invoice.Core.Common.Abstractions;

namespace Invoice.Core.Purchase.Entities;

public class PurchaseInvoiceLine : TransactionLine
{
    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
}

using Invoice.Core.Common.Abstractions;
using Invoice.Core.Common.Values;

namespace Invoice.Core.Sale.Entities;

public class SaleInvoice : Transaction
{
    public int CustomerId { get; set; }
    public string Number { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<SaleInvoiceLine> Lines { get; } = [];
    public List<SaleInvoiceTax> Taxes { get; } = [];
}

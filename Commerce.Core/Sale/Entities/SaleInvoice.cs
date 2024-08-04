using Commerce.Core.Common.Abstractions;
using Commerce.Core.Common.Values;

namespace Commerce.Core.Sale.Entities;

public class SaleInvoice : Transaction
{
    public int CustomerId { get; set; }
    public DateTime DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public InvoiceStatus Status { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<SaleInvoiceLine> Lines { get; } = [];
    public List<SaleInvoiceTax> Taxes { get; } = [];
}

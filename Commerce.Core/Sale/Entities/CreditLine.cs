using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditLine : LineItem
{
    public int CreditNoteId { get; set; }

    public CreditNote Note { get; set; } = null!;
    public List<CreditLineTax> Taxes { get; set; } = []!;
}

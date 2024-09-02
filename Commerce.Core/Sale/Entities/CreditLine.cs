using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditLine : LineItem<CreditLineTax>
{
    public int CreditNoteId { get; set; }
    public CreditNote Note { get; set; } = null!;
}

using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditLine : TransactionLine<CreditLineTax>
{
    public int CreditNoteId { get; set; }
    public CreditNote Note { get; set; } = null!;
}

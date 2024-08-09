using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class CreditTax : TransactionTax
{
    public int CreditNoteId { get; set; }

    public CreditNote Note { get; set; } = null!;
}

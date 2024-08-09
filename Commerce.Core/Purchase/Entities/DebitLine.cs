using Commerce.Core.Common.Abstractions;
using Commerce.Core.Sale.Entities;

namespace Commerce.Core.Purchase.Entities;

public class DebitLine : TransactionLine
{
    public int CreditNoteId { get; set; }

    public CreditNote Note { get; set; } = null!;
}

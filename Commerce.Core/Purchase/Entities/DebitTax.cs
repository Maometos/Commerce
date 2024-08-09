using Commerce.Core.Common.Abstractions;
using Commerce.Core.Sale.Entities;

namespace Commerce.Core.Purchase.Entities;

public class DebitTax : TransactionTax
{
    public int CreditNoteId { get; set; }

    public CreditNote Note { get; set; } = null!;
}

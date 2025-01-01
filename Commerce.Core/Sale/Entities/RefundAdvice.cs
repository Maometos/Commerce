using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Sale.Entities;

public class RefundAdvice : Payment
{
    public int CreditNoteId { get; set; }

    public CreditNote Note { get; set; } = null!;
}

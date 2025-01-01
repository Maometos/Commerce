using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class RefundReceipt : Payment
{
    public int DebitNoteId { get; set; }

    public DebitNote Note { get; set; } = null!;
}

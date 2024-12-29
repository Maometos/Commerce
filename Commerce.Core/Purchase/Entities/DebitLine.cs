using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class DebitLine : TransactionLine<DebitLineTax>
{
    public int DebitNoteId { get; set; }
    public DebitNote Note { get; set; } = null!;
}

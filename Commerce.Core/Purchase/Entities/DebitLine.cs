using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Purchase.Entities;

public class DebitLine : LineItem
{
    public int DebitNoteId { get; set; }

    public DebitNote Note { get; set; } = null!;
    public List<DebitLineTax> Taxes { get; set; } = []!;

}

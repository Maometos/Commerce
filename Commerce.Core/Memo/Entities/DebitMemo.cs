using Commerce.Core.Common.Abstractions;
using Commerce.Core.Contacts.Entities;

namespace Commerce.Core.Memo.Entities;

public class DebitMemo : Memo<DebitLine, DebitLineTax>
{
    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;
    public List<RefundReceipt> Refunds { get; } = [];
}

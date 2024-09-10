using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Common.Entities;

public class Tax : Entity
{
    public int TaxGroupId { get; set; }
    public decimal Rate { get; set; }
    public TaxGroup Group { get; set; } = null!;
}

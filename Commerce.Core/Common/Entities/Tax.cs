using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Entities;

public class Tax : Entity
{
    public int TaxGroupId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
    public TaxGroup Group { get; set; } = null!;
}

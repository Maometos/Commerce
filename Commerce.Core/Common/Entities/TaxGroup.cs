using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Common.Entities;

public class TaxGroup : Entity
{
    public List<Tax> Taxes { get; } = [];
}

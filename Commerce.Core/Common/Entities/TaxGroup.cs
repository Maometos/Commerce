using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Common.Entities;

public class TaxGroup : Definition
{
    public List<TaxRate> Rates { get; } = [];
}

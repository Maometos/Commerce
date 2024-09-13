using Commerce.Core.Common.Abstractions;

namespace Commerce.Core.Common.Entities;

public class TaxGroup : Profile
{
    public List<TaxRate> Rates { get; } = [];
}

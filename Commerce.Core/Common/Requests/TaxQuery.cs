using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Requests;

public class TaxQuery : Query
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

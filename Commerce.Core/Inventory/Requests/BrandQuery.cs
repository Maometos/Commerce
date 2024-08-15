using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class BrandQuery : Query
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

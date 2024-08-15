using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class CategoryQuery : Query
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

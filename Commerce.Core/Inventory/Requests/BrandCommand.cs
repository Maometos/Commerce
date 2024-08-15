using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class BrandCommand : Command
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

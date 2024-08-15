using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class CategoryCommand : Command
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

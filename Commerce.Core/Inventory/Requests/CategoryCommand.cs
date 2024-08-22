using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class CategoryCommand : Command
{
    public object? Argument { get; set; }
}

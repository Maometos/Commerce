using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class BrandCommand : Command
{
    public object? Argument { get; set; }
}

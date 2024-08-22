using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Purchase.Requests;

public class VendorCommand : Command
{
    public object? Argument { get; set; }
}

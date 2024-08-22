using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Sale.Requests;

public class CustomerCommand : Command
{
    public object? Argument { get; set; }
}

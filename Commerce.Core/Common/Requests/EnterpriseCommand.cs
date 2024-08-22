using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Requests;

public class EnterpriseCommand : Command
{
    public object? Argument { get; set; }
}

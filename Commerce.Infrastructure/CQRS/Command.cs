using Commerce.Infrastructure.Dispatcher;

namespace Commerce.Infrastructure.CQRS;

public abstract class Command : Request
{
    public CommandAction Action { get; set; } = CommandAction.None;
}

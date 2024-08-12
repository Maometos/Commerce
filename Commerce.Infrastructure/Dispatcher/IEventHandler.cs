namespace Commerce.Infrastructure.Dispatcher;

public interface IEventHandler
{
    public Task<object?> InvokeAsync(IEvent eventArgs, CancellationToken token);
}

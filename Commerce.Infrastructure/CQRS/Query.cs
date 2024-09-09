using Commerce.Infrastructure.Dispatcher;

namespace Commerce.Infrastructure.CQRS;

public abstract class Query : Request
{
    public Dictionary<string, object> Parameters { get; set; } = [];
    public string Sort { get; set; } = null!;
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 0;
}

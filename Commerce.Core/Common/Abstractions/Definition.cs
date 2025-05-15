using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Abstractions;

public abstract class Definition : Entity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

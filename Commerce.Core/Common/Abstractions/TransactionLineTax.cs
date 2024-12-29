using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Abstractions;

public abstract class TransactionLineTax : Entity
{
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
}

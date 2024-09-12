using Commerce.Core.Common.Entities;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Abstractions;

public abstract class Document : Entity
{
    public int EnterpriseId { get; set; }
    public string Reference { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.Now;

    public Enterprise Enterprise { get; set; } = null!;
}

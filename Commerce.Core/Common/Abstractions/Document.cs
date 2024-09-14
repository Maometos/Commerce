using Commerce.Core.Common.Entities;
using Commerce.Core.Common.Values;
using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Abstractions;

public abstract class Document : Entity
{
    public int EnterpriseId { get; set; }
    public string Reference { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.Now;
    public DocumentStatus DocumentStatus { get; set; } = DocumentStatus.Draft;

    public Enterprise Enterprise { get; set; } = null!;
}

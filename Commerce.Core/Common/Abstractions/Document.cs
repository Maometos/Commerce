using Commerce.Core.Common.Entities;

namespace Commerce.Core.Common.Abstractions;

public abstract class Document
{
    public int Id { get; set; }
    public int EnterpriseId { get; set; }
    public string Reference { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.Now;

    public Enterprise Enterprise { get; set; } = null!;
}

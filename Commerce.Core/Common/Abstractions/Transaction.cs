using Commerce.Core.Organization.Entities;

namespace Commerce.Core.Common.Abstractions;

public abstract class Transaction
{
    public int Id { get; set; }
    public int EnterpriseId { get; set; }
    public string Reference { get; set; } = null!;
    public DateTime Date { get; set; }

    public Enterprise Enterprise { get; set; } = null!;
}

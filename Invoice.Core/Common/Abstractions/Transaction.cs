using Invoice.Core.Organization.Entities;

namespace Invoice.Core.Common.Abstractions;

public abstract class Transaction
{
    public int Id { get; set; }
    public int EnterpriseId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }

    public Enterprise Enterprise { get; set; } = null!;
}

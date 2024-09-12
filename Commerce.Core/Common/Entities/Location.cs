using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Common.Entities;

public class Location : Entity
{
    public string Address { get; set; } = null!;
    public string Locality { get; set; } = null!;
    public string Territory { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
}

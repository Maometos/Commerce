using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Sale.Requests;

public class CustomerCommand : Command
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Locality { get; set; }
    public string? Territory { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
}

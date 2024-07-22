namespace Invoice.Core.Common.Entities;

public class Location
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string Locality { get; set; } = null!;
    public string Territory { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
}

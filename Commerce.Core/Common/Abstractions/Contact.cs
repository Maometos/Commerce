namespace Commerce.Core.Common.Abstractions;

public abstract class Contact : Entity
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Locality { get; set; }
    public string? Territory { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
}

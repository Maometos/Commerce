using Invoice.Core.Common.Entities;

namespace Invoice.Core.Common.Abstractions;

public abstract class Profile
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public Location Location { get; set; } = null!;
}

namespace Commerce.Core.Common.Abstractions;

public abstract class Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

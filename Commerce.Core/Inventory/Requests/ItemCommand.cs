using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class ItemCommand : Command
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
}

using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class ItemQuery : Query
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }
    public decimal? Price { get; set; }
}

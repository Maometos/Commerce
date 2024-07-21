using Microsoft.EntityFrameworkCore;

namespace Invoice.Core.Common;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}

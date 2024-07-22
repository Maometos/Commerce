﻿using Invoice.Core.Common.Entities;
using Invoice.Core.Organization.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Core.Common;

public class DataContext : DbContext
{
    public DbSet<Enterprise> Enterprises { get; set; }
    public DbSet<Location> Locations { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}

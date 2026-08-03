using Microsoft.EntityFrameworkCore;
using Steam2Api.Entities;

namespace Steam2Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<GameEntity> Games { get; set; }

    public DbSet<InvoiceEntity> Invoices { get; set; }

    public DbSet<InvoiceDetailEntity> InvoiceDetails { get; set; }
}

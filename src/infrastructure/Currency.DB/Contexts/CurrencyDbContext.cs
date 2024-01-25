using Microsoft.EntityFrameworkCore;

namespace Currency.DB;

public class CurrencyDbContext : DbContext
{
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
    {
    }

    public DbSet<CurrencyRate> CurrencyRates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasPostgresEnum<CurrencyType>();

        modelBuilder.Entity<CurrencyRate>()
            .HasKey(c => c.Id);
    }
}
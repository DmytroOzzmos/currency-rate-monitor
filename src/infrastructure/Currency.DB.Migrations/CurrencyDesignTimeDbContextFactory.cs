using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Currency.DB.Migrations;

public class CurrencyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CurrencyDbContext>
{
    public CurrencyDbContext CreateDbContext(string[] args)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        dataSourceBuilder.MapEnum<CurrencyType>();
        var dataSource = dataSourceBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<CurrencyDbContext>();
        optionsBuilder.UseNpgsql(dataSource, builder => builder.MigrationsAssembly("Currency.DB.Migrations"))
            .UseSnakeCaseNamingConvention();

        return new CurrencyDbContext(optionsBuilder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Currency.DB.Migrations;

public static class DependencyInjection
{
    public static void AddCurrencyDbMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CurrencyDbContext>(options =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetSection("DATABASE").Value);
            dataSourceBuilder.MapEnum<CurrencyType>();
            var dataSource = dataSourceBuilder.Build();

            options.UseNpgsql(dataSource, builder => builder.MigrationsAssembly("Currency.DB.Migrations"))
                .UseSnakeCaseNamingConvention();
        });
    }
}
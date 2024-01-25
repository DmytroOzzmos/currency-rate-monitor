using Currency.DB;
using Currency.DB.Migrations;
using Microsoft.EntityFrameworkCore;

namespace CurrencyStat.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "CurrencyStat.Api", Version = "v1" });
        });

        services.AddScoped<ICurrencyRateService, CurrencyRateService>();
        services.AddScoped<ICurrencyRateGenerator, CurrencyRateGenerator>();
        services.AddHostedService<CurrencyRateInfoBackgroundService>();

        services.Configure<CurrencyInfoConfig>(_configuration.GetSection("CurrencyInfoConfig"));
        services.AddCurrencyDbMigrations(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var dbContext = app.ApplicationServices.CreateAsyncScope()
            .ServiceProvider.GetService<CurrencyDbContext>();
        dbContext.Database.Migrate();

        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyStat.Api v1"));

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

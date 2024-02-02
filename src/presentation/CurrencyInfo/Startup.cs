using FastEndpoints;

namespace CurrencyInfo.WS;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFastEndpoints();

        services.AddScoped<ICurrencyRateGenerator, CurrencyRateGenerator>();
        services.AddScoped<INBUService, NBUService>();

        services.AddLogging(c => c.AddConsole());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseWebSockets();
        app.UseFastEndpoints();
    }
}

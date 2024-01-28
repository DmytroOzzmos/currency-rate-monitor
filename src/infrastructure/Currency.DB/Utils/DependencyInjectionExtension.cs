using Microsoft.Extensions.DependencyInjection;

namespace Currency.DB;

public static class DependencyInjectionExtension
{
    public static void AddCurrencyDb(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddCurrencyRateCommandHandler>());
    }
}
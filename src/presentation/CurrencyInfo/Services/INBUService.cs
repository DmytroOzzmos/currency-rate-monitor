namespace CurrencyInfo.WS;

public interface INBUService
{
    Task<CurrencyRate> GetCurrencyRateAsync();
}
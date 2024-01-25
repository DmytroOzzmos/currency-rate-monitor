namespace CurrencyStat.Api;

public interface ICurrencyRateService
{
    Task Add(CurrencyRateInfo currencyRate);
    Task<CurrencyRateDetails> GetDetailsAsync(GetCurrencyRateModel model);
    Task GeneratedDataAsync();
}
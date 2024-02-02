namespace CurrencyInfo.WS;

public class NBUService : INBUService
{
    private readonly ICurrencyRateGenerator _currencyRateGenerator;

    public NBUService(ICurrencyRateGenerator currencyRateGenerator)
    {
        _currencyRateGenerator = currencyRateGenerator;
    }

    public async Task<CurrencyRate> GetCurrencyRateAsync()
    {
        await Task.Delay(200);

        var result = _currencyRateGenerator.GetCurrencyRateFaker().Generate();
        return result;
    }
}
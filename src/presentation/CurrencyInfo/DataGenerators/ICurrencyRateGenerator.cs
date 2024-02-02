using Bogus;

namespace CurrencyInfo.WS;

public interface ICurrencyRateGenerator
{
    Faker<CurrencyRate> GetCurrencyRateFaker();
}
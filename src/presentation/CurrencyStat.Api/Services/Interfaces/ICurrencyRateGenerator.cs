using Bogus;
using Currency.DB;

namespace CurrencyStat.Api;

public interface ICurrencyRateGenerator
{
    Faker<CurrencyRate> GetCurrencyRateFaker();
}
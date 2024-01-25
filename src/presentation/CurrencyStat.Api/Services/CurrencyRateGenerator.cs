using Bogus;
using Currency.DB;

namespace CurrencyStat.Api;

public class CurrencyRateGenerator : ICurrencyRateGenerator
{
    public Faker<CurrencyRate> GetCurrencyRateFaker()
    {
        var currencyRateFaker = new Faker<CurrencyRate>()
            .Rules((f, cr) =>
            {
                cr.Id = Guid.NewGuid();
                cr.FromCurrency = f.Random.Enum<CurrencyType>();
                cr.ToCurrency = f.PickRandomWithout(cr.FromCurrency);
                cr.Price = GetRandomPrice(f, cr.FromCurrency, cr.ToCurrency);
                cr.Timestamp = f.Date.Between(DateTime.UtcNow.AddMonths(-2), DateTime.UtcNow);
            });

        return currencyRateFaker;
    }

    private double GetRandomPrice(Faker faker, CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var fromCurrencyPrice = GetRandomPriceInRelationUAH(faker, fromCurrency);
        var toCurrencyPrice = GetRandomPriceInRelationUAH(faker, toCurrency);

        var result = Math.Round(fromCurrencyPrice / toCurrencyPrice, 4);
        return result;
    }

    private double GetRandomPriceInRelationUAH(Faker faker, CurrencyType currency)
    {
        return currency switch
        {
            CurrencyType.USD => faker.Random.Number(36, 43) + faker.Random.Double(),
            CurrencyType.EUR => faker.Random.Number(37, 45) + faker.Random.Double(),
            CurrencyType.PLN => faker.Random.Number(7, 9) + faker.Random.Double(),
            CurrencyType.UAH => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null)
        };
    }
}
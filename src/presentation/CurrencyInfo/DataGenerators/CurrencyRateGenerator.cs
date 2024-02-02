using Bogus;

namespace CurrencyInfo.WS;

public class CurrencyRateGenerator : ICurrencyRateGenerator
{
    public Faker<CurrencyRate> GetCurrencyRateFaker()
    {
        return new Faker<CurrencyRate>()
            .Rules((f, c) =>
            {
                var fromCurrency = f.Random.Enum<CurrencyType>();
                var toCurrency = f.PickRandomWithout(fromCurrency);
                c.Currency = $"{fromCurrency}/{toCurrency}";

                c.Price = GetRandomPrice(f, fromCurrency, toCurrency);
                c.Timestamp = DateTime.UtcNow;
            });
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
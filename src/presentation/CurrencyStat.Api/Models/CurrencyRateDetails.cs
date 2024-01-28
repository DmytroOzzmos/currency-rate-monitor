using Currency.DB;

namespace CurrencyStat.Api;

public class CurrencyRateDetails
{
    public string Currency { get; set; }
    public int Count { get; set; }
    public IEnumerable<CurrencyRateByInterval> Data { get; set; }
}
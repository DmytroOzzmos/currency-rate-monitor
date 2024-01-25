using Currency.DB;

namespace CurrencyStat.Api;

public class GetCurrencyRateModel
{
    public CurrencyType FromCurrency { get; set; }
    public CurrencyType ToCurrency { get; set; }
    public IntervalType IntervalType { get; set; }
}
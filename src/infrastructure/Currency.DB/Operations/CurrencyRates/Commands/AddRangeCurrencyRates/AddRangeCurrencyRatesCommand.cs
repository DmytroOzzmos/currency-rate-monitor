using MediatR;

namespace Currency.DB;

public class AddRangeCurrencyRatesCommand : IRequest<Unit>
{
    public IEnumerable<CurrencyRate> CurrencyRates { get; set; }
}
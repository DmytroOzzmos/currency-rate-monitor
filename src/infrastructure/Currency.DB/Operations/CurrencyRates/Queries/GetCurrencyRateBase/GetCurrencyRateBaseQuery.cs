using MediatR;

namespace Currency.DB;

public abstract class GetCurrencyRateBaseQuery : IRequest<List<CurrencyRateByInterval>>
{
    public CurrencyType FromCurrency { get; set; }
    public CurrencyType ToCurrency { get; set; }
}
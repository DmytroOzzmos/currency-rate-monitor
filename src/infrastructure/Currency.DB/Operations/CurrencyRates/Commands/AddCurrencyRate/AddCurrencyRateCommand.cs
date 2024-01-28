using MediatR;

namespace Currency.DB;

public class AddCurrencyRateCommand : IRequest<Unit>
{
    public CurrencyRate CurrencyRate { get; set; }
}
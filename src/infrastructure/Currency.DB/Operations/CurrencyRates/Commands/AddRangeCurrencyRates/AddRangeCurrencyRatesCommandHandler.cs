using MediatR;

namespace Currency.DB;

public class AddRangeCurrencyRatesCommandHandler : IRequestHandler<AddRangeCurrencyRatesCommand, Unit>
{
    private readonly CurrencyDbContext _currencyDbContext;

    public AddRangeCurrencyRatesCommandHandler(CurrencyDbContext currencyDbContext)
    {
        _currencyDbContext = currencyDbContext;
    }

    public async Task<Unit> Handle(AddRangeCurrencyRatesCommand request, CancellationToken cancellationToken)
    {
        await _currencyDbContext.CurrencyRates.AddRangeAsync(request.CurrencyRates, cancellationToken);
        await _currencyDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
using MediatR;

namespace Currency.DB;

public class AddCurrencyRateCommandHandler : IRequestHandler<AddCurrencyRateCommand, Unit>
{
    private readonly CurrencyDbContext _currencyDbContext;

    public AddCurrencyRateCommandHandler(CurrencyDbContext currencyDbContext)
    {
        _currencyDbContext = currencyDbContext;
    }

    public async Task<Unit> Handle(AddCurrencyRateCommand request, CancellationToken cancellationToken)
    {
        await _currencyDbContext.CurrencyRates.AddAsync(request.CurrencyRate, cancellationToken);
        await _currencyDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
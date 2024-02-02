using MediatR;

namespace Currency.DB;

public abstract class GetCurrencyRateBaseQueryHandler<TRequest> : IRequestHandler<TRequest, List<CurrencyRateByInterval>>
    where TRequest : GetCurrencyRateBaseQuery
{
    protected readonly CurrencyDbContext _currencyDbContext;

    public GetCurrencyRateBaseQueryHandler(CurrencyDbContext currencyDbContext)
    {
        _currencyDbContext = currencyDbContext;
    }

    public abstract Task<List<CurrencyRateByInterval>> Handle(TRequest request, CancellationToken cancellationToken);

    public IQueryable<CurrencyRate> GetFilter(TRequest request)
    {
        var result = _currencyDbContext.CurrencyRates
            .Where(x => x.FromCurrency == request.FromCurrency && x.ToCurrency == request.ToCurrency);

        return result;
    }

}
using Microsoft.EntityFrameworkCore;

namespace Currency.DB;

public class GetCurrencyRateByOneHourQueryHandler : GetCurrencyRateBaseQueryHandler<GetCurrencyRateByOneHourQuery>
{
    public GetCurrencyRateByOneHourQueryHandler(CurrencyDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<CurrencyRateByInterval>> Handle(GetCurrencyRateByOneHourQuery request, CancellationToken cancellationToken)
    {
        var currencyRateByIntervals = GetFilter(request)
            .GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, 0, 0),
                Interval = IntervalType.OneHour,
            })
            .ToListAsync(cancellationToken);

        return currencyRateByIntervals;
    }
}
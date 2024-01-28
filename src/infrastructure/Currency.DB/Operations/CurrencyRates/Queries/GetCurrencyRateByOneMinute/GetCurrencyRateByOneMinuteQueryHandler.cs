using Microsoft.EntityFrameworkCore;

namespace Currency.DB;

public class GetCurrencyRateByOneMinuteQueryHandler : GetCurrencyRateBaseQueryHandler<GetCurrencyRateByOneMinuteQuery>
{
    public GetCurrencyRateByOneMinuteQueryHandler(CurrencyDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<List<CurrencyRateByInterval>> Handle(GetCurrencyRateByOneMinuteQuery request, CancellationToken cancellationToken)
    {
        var currencyRateByIntervals = await GetFilter(request)
            .GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour, x.Timestamp.Minute })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, x.Key.Minute, 0),
                Interval = IntervalType.OneMinute,
            })
            .ToListAsync(cancellationToken);

        return currencyRateByIntervals;
    }
}
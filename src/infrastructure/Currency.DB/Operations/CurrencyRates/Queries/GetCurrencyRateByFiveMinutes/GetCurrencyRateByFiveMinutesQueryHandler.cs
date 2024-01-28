using Microsoft.EntityFrameworkCore;

namespace Currency.DB;

public class GetCurrencyRateByFiveMinutesQueryHandler : GetCurrencyRateBaseQueryHandler<GetCurrencyRateByFiveMinutesQuery>
{
    public GetCurrencyRateByFiveMinutesQueryHandler(CurrencyDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<CurrencyRateByInterval>> Handle(GetCurrencyRateByFiveMinutesQuery request, CancellationToken cancellationToken)
    {
        var currencyRateByIntervals = GetFilter(request)
            .GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour, Minute = x.Timestamp.Minute / 5 })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, x.Key.Minute * 5, 0),
                Interval = IntervalType.FiveMinutes,
            })
            .ToListAsync(cancellationToken);
            
        return currencyRateByIntervals;
    }
}
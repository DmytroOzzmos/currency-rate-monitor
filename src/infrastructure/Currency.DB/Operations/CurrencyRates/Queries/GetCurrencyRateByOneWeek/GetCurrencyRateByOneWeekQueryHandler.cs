using Microsoft.EntityFrameworkCore;

namespace Currency.DB;

public class GetCurrencyRateByOneWeekQueryHandler : GetCurrencyRateBaseQueryHandler<GetCurrencyRateByOneWeekQuery>
{
    public GetCurrencyRateByOneWeekQueryHandler(CurrencyDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<List<CurrencyRateByInterval>> Handle(GetCurrencyRateByOneWeekQuery request, CancellationToken cancellationToken)
    {
        var currencyRateByIntervals = GetFilter(request)
            .GroupBy(x => new { x.Timestamp.Year, Week = x.Timestamp.DayOfYear / 7  })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, 1, 1).AddDays(x.Key.Week * 7),
                Interval = IntervalType.OneWeek,
            })
            .ToListAsync(cancellationToken);

        return currencyRateByIntervals;
    }
}
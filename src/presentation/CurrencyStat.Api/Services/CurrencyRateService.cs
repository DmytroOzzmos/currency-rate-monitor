using Currency.DB;
using Microsoft.EntityFrameworkCore;

namespace CurrencyStat.Api;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly CurrencyDbContext _currencyDbContext;
    private readonly ICurrencyRateGenerator _currencyRateGenerator;

    public CurrencyRateService(CurrencyDbContext currencyDbContext, ICurrencyRateGenerator currencyRateGenerator)
    {
        _currencyDbContext = currencyDbContext;
        _currencyRateGenerator = currencyRateGenerator;
    }

    public async Task Add(CurrencyRateInfo currencyRate)
    {
        var currencies = currencyRate.Currency.Split("/");
        var fromCurrency = Enum.Parse<CurrencyType>(currencies.First());
        var toCurrency = Enum.Parse<CurrencyType>(currencies.Last());

        var currencyRateEntity = new CurrencyRate
        {
            FromCurrency = fromCurrency,
            ToCurrency = toCurrency,
            Price = currencyRate.Price,
            Timestamp = currencyRate.Timestamp.ToUniversalTime(),
        };

        await _currencyDbContext.CurrencyRates.AddAsync(currencyRateEntity);
        await _currencyDbContext.SaveChangesAsync();
    }

    public async Task<CurrencyRateDetails> GetDetailsAsync(GetCurrencyRateModel model)
    {
        var fromCurrency = CurrencyType.USD;
        var toCurrency = CurrencyType.UAH;

        var currencyRateByIntervals = model.IntervalType switch
        {
            IntervalType.OneMinute => await GetCurrencyRateByOneMinuteAsync(fromCurrency, toCurrency),
            IntervalType.FiveMinutes => await GetCurrencyRateByFiveMinutesAsync(fromCurrency, toCurrency),
            IntervalType.OneHour => await GetCurrencyRateByOneHourAsync(fromCurrency, toCurrency),
            IntervalType.OneDay => await GetCurrencyRateByOneDayAsync(fromCurrency, toCurrency),
            IntervalType.OneWeek => await GetCurrencyRateByOneWeekAsync(fromCurrency, toCurrency),
            _ => throw new ArgumentOutOfRangeException(nameof(model.IntervalType), model.IntervalType, null),
        };

        var result = new CurrencyRateDetails
        {
            Currency = $"{fromCurrency}/{toCurrency}",
            Count = currencyRateByIntervals.Count,
            Data = currencyRateByIntervals,
        };
        return result;
    }

    public async Task GeneratedDataAsync()
    {
        var currencyRateFaker = _currencyRateGenerator.GetCurrencyRateFaker();
        var currencyRates = currencyRateFaker.Generate(10000);

        await _currencyDbContext.CurrencyRates.AddRangeAsync(currencyRates);
        await _currencyDbContext.SaveChangesAsync();
    }

    private async Task<List<CurrencyRateByInterval>> GetCurrencyRateByOneMinuteAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var currencyRateByIntervals = await GetFilter(fromCurrency, toCurrency)
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
            .ToListAsync();

        return currencyRateByIntervals;
    }

    private async Task<List<CurrencyRateByInterval>> GetCurrencyRateByFiveMinutesAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var currencyRateByIntervals = await GetFilter(fromCurrency, toCurrency)
            .GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour, Minute = x.Timestamp.Minute / 5 })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, x.Key.Minute * 5, 0),
                Interval = IntervalType.OneMinute,
            })
            .ToListAsync();

        return currencyRateByIntervals;
    }

    private async Task<List<CurrencyRateByInterval>> GetCurrencyRateByOneHourAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var currencyRateByIntervals = await GetFilter(fromCurrency, toCurrency)
            .GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, 0, 0),
                Interval = IntervalType.OneMinute,
            })
            .ToListAsync();

        return currencyRateByIntervals;
    }

    private async Task<List<CurrencyRateByInterval>> GetCurrencyRateByOneDayAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var currencyRateByIntervals = await GetFilter(fromCurrency, toCurrency)
            .GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day),
                Interval = IntervalType.OneDay,
            })
            .ToListAsync();

        return currencyRateByIntervals;
    }

    private async Task<List<CurrencyRateByInterval>> GetCurrencyRateByOneWeekAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var currencyRateByIntervals = await GetFilter(fromCurrency, toCurrency)
            .GroupBy(x => new { x.Timestamp.Year, Week = x.Timestamp.DayOfYear / 7 })
            .Select(x => new CurrencyRateByInterval
            {
                Low = x.Min(x => x.Price),
                High = x.Max(x => x.Price),
                Open = x.OrderBy(x => x.Timestamp).First().Price,
                Close = x.OrderBy(x => x.Timestamp).Last().Price,
                Count = x.Count(),
                Timestamp = new DateTime(x.Key.Year, 1, 1).AddDays(x.Key.Week * 7),
                Interval = IntervalType.OneMinute,
            })
            .ToListAsync();

        return currencyRateByIntervals;
    }

    private IQueryable<CurrencyRate> GetFilter(CurrencyType fromCurrency, CurrencyType toCurrency)
    {
        var result = _currencyDbContext.CurrencyRates
            .Where(x => x.FromCurrency == fromCurrency && x.ToCurrency == toCurrency);

        return result;
    }
}
using Currency.DB;
using MediatR;

namespace CurrencyStat.Api;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyRateGenerator _currencyRateGenerator;
    private readonly IMediator _mediator;

    public CurrencyRateService(ICurrencyRateGenerator currencyRateGenerator, IMediator mediator)
    {
        _currencyRateGenerator = currencyRateGenerator;
        _mediator = mediator;
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

        await _mediator.Send(new AddCurrencyRateCommand
        {
            CurrencyRate = currencyRateEntity,
        });
    }

    public async Task<CurrencyRateDetails> GetDetailsAsync(GetCurrencyRateModel model)
    {
        var fromCurrency = CurrencyType.USD;
        var toCurrency = CurrencyType.UAH;

        var currencyRateByIntervals = await GetCurrencyRateByIntervalsAsync(model);

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

        await _mediator.Send(new AddRangeCurrencyRatesCommand
        {
            CurrencyRates = currencyRates,
        });
    }


    private async Task<List<CurrencyRateByInterval>> GetCurrencyRateByIntervalsAsync(GetCurrencyRateModel model)
    {
        var currencyRateByIntervals = model.IntervalType switch
        {
            IntervalType.OneMinute => await _mediator.Send(new GetCurrencyRateByOneMinuteQuery
            {
                FromCurrency = model.FromCurrency,
                ToCurrency = model.ToCurrency,
            }),
            IntervalType.FiveMinutes => await _mediator.Send(new GetCurrencyRateByFiveMinutesQuery
            {
                FromCurrency = model.FromCurrency,
                ToCurrency = model.ToCurrency,
            }),
            IntervalType.OneHour => await _mediator.Send(new GetCurrencyRateByOneHourQuery
            {
                FromCurrency = model.FromCurrency,
                ToCurrency = model.ToCurrency,
            }),
            IntervalType.OneDay => await _mediator.Send(new GetCurrencyRateByOneDayQuery
            {
                FromCurrency = model.FromCurrency,
                ToCurrency = model.ToCurrency,
            }),
            IntervalType.OneWeek => await _mediator.Send(new GetCurrencyRateByOneWeekQuery
            {
                FromCurrency = model.FromCurrency,
                ToCurrency = model.ToCurrency,
            }),
            _ => throw new ArgumentOutOfRangeException(nameof(model.IntervalType), model.IntervalType, null),
        };

        return currencyRateByIntervals;
    }
}
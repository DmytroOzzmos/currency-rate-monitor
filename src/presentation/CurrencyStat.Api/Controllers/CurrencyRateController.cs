using Microsoft.AspNetCore.Mvc;

namespace CurrencyStat.Api;

[ApiController]
[Route("api/[controller]")]
public class CurrencyRateController : ControllerBase
{
    private readonly ICurrencyRateService _currencyRateService;

    public CurrencyRateController(ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType(typeof(CurrencyRateDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCurrencyRateAsync([FromQuery] GetCurrencyRateModel model)
    {
        if (model.FromCurrency == model.ToCurrency)
            return BadRequest("FromCurrency and ToCurrency must be different.");
        
        var result = await _currencyRateService.GetDetailsAsync(model);

        return Ok(result);
    }

    [HttpPost]
    [Route("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateCurrencyRateAsync()
    {
        await _currencyRateService.GeneratedDataAsync();

        return Ok();
    }
}
using System.Net.WebSockets;
using Microsoft.Extensions.Options;
using ProtoBuf;

namespace CurrencyStat.Api;

public class CurrencyRateInfoBackgroundService : BackgroundService
{
    private readonly ICurrencyRateService _currencyRateService;
    private readonly IServiceScope _serviceScope;
    private readonly ILogger<CurrencyRateInfoBackgroundService> _logger;
    private readonly CurrencyInfoConfig _currencyInfoConfig;

    public CurrencyRateInfoBackgroundService(ILogger<CurrencyRateInfoBackgroundService> logger, IServiceProvider serviceProvider, IOptions<CurrencyInfoConfig> currencyInfoConfigOptions)
    {
        _serviceScope = serviceProvider.CreateScope();
        _logger = logger;
        _currencyRateService = _serviceScope.ServiceProvider.GetRequiredService<ICurrencyRateService>();
        _currencyInfoConfig = currencyInfoConfigOptions.Value;
    }

    protected async override Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation($"{nameof(CurrencyRateInfoBackgroundService)} is running.");

        while (!ct.IsCancellationRequested)
        {
            try
            {
                await BackgroundProcessing(ct);
            }
            catch (WebSocketException ex)
            {
                _logger.LogError(ex, "Error occurred executing {ServiceName}.", nameof(CurrencyRateInfoBackgroundService));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing {ServiceName}.", nameof(CurrencyRateInfoBackgroundService));
                throw ex;
            }

            await Task.Delay(TimeSpan.FromSeconds(10), ct);
        }
    }

    private async Task BackgroundProcessing(CancellationToken ct)
    {
        using var ws = new ClientWebSocket();

        _logger.LogInformation("Connecting to CurrencyInfo WS Server.");
        await ws.ConnectAsync(new Uri($"ws://{_currencyInfoConfig.Host}{_currencyInfoConfig.GetCurrencyInfoEndpoint}"), CancellationToken.None);
        _logger.LogInformation("Connected to CurrencyInfo WS Server.");

        await ReceiveAsync(ws, ct);

        _logger.LogInformation("Disconnecting from CurrencyInfo WS Server.");
        if (ws.State != WebSocketState.Aborted && ws.State != WebSocketState.Closed)
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }

    private async Task ReceiveAsync(WebSocket ws, CancellationToken ct)
    {
        var buffer = new byte[1024];
        while ((ws.State == WebSocketState.Open || ws.State == WebSocketState.Connecting) && !ct.IsCancellationRequested)
        {
            try
            {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                var ms = new MemoryStream(buffer, 0, result.Count);
                var model = Serializer.Deserialize<CurrencyRateInfo>(ms);

                await _currencyRateService.Add(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing {ServiceName}.", nameof(CurrencyRateInfoBackgroundService));
                break;
            }
        }
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        _logger.LogInformation($"{nameof(CurrencyRateInfoBackgroundService)} is stopping.");

        _serviceScope.Dispose();

        await base.StopAsync(ct);
    }
}
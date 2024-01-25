using System.Net.WebSockets;
using FastEndpoints;
using ProtoBuf;

namespace CurrencyInfo.WS;

public class CurrencyRateWSEndpoint : EndpointWithoutRequest
{
    private readonly INBUService _nbuService;
    private readonly ILogger<CurrencyRateWSEndpoint> _logger;

    public CurrencyRateWSEndpoint(INBUService nbuService, ILogger<CurrencyRateWSEndpoint> logger)
    {
        _nbuService = nbuService;
        _logger = logger;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("ws/getCurrencyInfo");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            await ExecuteWebSocketAsync();
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task ExecuteWebSocketAsync()
    {
        _logger.LogInformation("Open connection");
        var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await Task.WhenAll(SendAsync(webSocket), ReceiveAsync(webSocket));

        _logger.LogInformation("Close connection");
        if (webSocket.State != WebSocketState.Aborted)
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }

    private async Task SendAsync(WebSocket webSocket)
    {

        while (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.Connecting)
        {
            var currencyRate = await _nbuService.GetCurrencyRateAsync();
            
            using var mr = new MemoryStream();
            Serializer.Serialize(mr, currencyRate);

            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(mr.ToArray(), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
    }

    private async Task ReceiveAsync(WebSocket webSocket)
    {
        try
        {
            while (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.Connecting)
            {
                var buffer = new byte[1024];
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }
        catch (WebSocketException ex)
        {
            _logger.LogError(ex, $"WebSocketException: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception: {ex.Message}");
            throw;
        }
    }
}
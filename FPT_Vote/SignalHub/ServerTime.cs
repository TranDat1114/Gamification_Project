using FPT_Vote.SignalHub;
using Microsoft.AspNetCore.SignalR;

namespace FPT_Vote.SignalHub;

public class ServerTime : BackgroundService
{
    private static readonly TimeSpan period = TimeSpan.FromSeconds(5);
    private readonly ILogger<ServerTime> _logger;
    private readonly IHubContext<RealTimeHub, IRealTimeClient> _hubContext;

    public ServerTime(ILogger<ServerTime> logger, IHubContext<RealTimeHub, IRealTimeClient> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(period);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.Now;
            _logger.LogInformation($"Server time = {dateTime:dd/MM/yyyy hh:mm:ss tt}", nameof(ServerTime), dateTime);
            await _hubContext.Clients.All.ReceiveMessage(@$"Server time = {dateTime:dd/MM/yyyy hh:mm:ss tt}");

        }

    }
}
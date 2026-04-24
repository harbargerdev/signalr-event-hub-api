using Microsoft.AspNetCore.SignalR;
using SignalR.Events.Api.Hubs;
using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Services;

public class CurrentUserEventWorker : BackgroundService
{
    private readonly IHubContext<EventHub> _hubContext;
    private readonly ILogger<CurrentUserEventWorker> _logger;
    private readonly Random _random = new();

    public CurrentUserEventWorker(
        IHubContext<EventHub> hubContext, 
        ILogger<CurrentUserEventWorker> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CurrentUserEventWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var currentUserId = Environment.UserName;

                var notification = new EventNotification
                {
                    UserId = currentUserId,
                    EventType = "newEvent"
                };

                await _hubContext.Clients.All.SendAsync("ReceiveEvent", notification, stoppingToken);

                _logger.LogInformation("Event broadcast for current user: {UserId}", currentUserId);

                var delaySeconds = _random.Next(10, 21);
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CurrentUserEventWorker");
            }
        }

        _logger.LogInformation("CurrentUserEventWorker stopped");
    }
}

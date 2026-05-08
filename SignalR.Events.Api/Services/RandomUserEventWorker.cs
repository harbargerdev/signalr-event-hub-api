using Microsoft.AspNetCore.SignalR;
using SignalR.Events.Api.Hubs;
using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Services;

public class RandomUserEventWorker : BackgroundService
{
    private readonly IHubContext<EventHub> _hubContext;
    private readonly ILogger<RandomUserEventWorker> _logger;
    private readonly Random _random = new();
    private readonly string _workerName;

    private static readonly string[] RandomUserNames = 
    {
        "alice", "bob", "charlie", "diana", "edward", "fiona", "george", "hannah",
        "isaac", "julia", "kevin", "laura", "michael", "nancy", "oliver", "patricia"
    };

    public RandomUserEventWorker(
        IHubContext<EventHub> hubContext, 
        ILogger<RandomUserEventWorker> logger,
        string workerName)
    {
        _hubContext = hubContext;
        _logger = logger;
        _workerName = workerName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{WorkerName} started", _workerName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var randomUserId = RandomUserNames[_random.Next(RandomUserNames.Length)];

                var notification = new EventNotification
                {
                    UserId = randomUserId,
                    EventType = "newEvent"
                };

                await _hubContext.Clients.All.SendAsync("ReceiveEvent", notification, stoppingToken);

                _logger.LogInformation("{WorkerName} - Event broadcast for random user: {UserId}", _workerName, randomUserId);

                var standardDelay = TimeSpan.FromMicroseconds(200);
                await Task.Delay(standardDelay, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {WorkerName}", _workerName);
            }
        }

        _logger.LogInformation("{WorkerName} stopped", _workerName);
    }
}

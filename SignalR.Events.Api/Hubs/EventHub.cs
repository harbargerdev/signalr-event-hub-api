using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Hubs;

public class EventHub : Hub
{
    private readonly ILogger<EventHub> _logger;

    public EventHub(ILogger<EventHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task BroadcastEvent(EventNotification notification)
    {
        _logger.LogInformation("Broadcasting event for userId: {UserId}, eventType: {EventType}", 
            notification.UserId, notification.EventType);
        await Clients.All.SendAsync("ReceiveEvent", JsonConvert.SerializeObject(notification));
    }
}

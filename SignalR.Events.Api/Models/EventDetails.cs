namespace SignalR.Events.Api.Models;

public class EventDetails
{
    public string UserId { get; set; } = string.Empty;
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public string EventName { get; set; } = string.Empty;
    public string EventDescription { get; set; } = string.Empty;
}

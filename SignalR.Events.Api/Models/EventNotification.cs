using Newtonsoft.Json;

namespace SignalR.Events.Api.Models;

public class EventNotification
{
    [JsonProperty("userId")]
    public string UserId { get; set; } = string.Empty;
    [JsonProperty("eventType")]
    public string EventType { get; set; } = string.Empty;
}

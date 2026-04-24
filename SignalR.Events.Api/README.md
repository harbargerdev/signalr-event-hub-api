# SignalR Events API

This API provides event management with real-time notifications via SignalR.

## Endpoints

### REST API

#### GET /events/query/{userId}/
Returns event details for a specific user.

**Response:**
```json
{
  "userId": "string",
  "eventId": "guid",
  "eventName": "string",
  "eventDescription": "string"
}
```

### SignalR Hub

#### /eventhub
WebSocket endpoint for real-time event notifications.

**Client Events:**
- `ReceiveEvent`: Receives event notifications with the following payload:
```json
{
  "userId": "string",
  "eventType": "newEvent"
}
```

## Background Workers

The API includes multiple background workers that automatically broadcast events:

1. **CurrentUserEventWorker**: Broadcasts events for the current user every 10-20 seconds (random interval)
   - UserId is configured in `appsettings.json` under `CurrentUserId`

2. **RandomUserEventWorker** (3 instances): Broadcast events for random users every 15-45 seconds (random interval)
   - Uses random userIds from a predefined list

## Configuration

Edit `appsettings.json` to configure the current user ID:

```json
{
  "CurrentUserId": "your-user-id-here"
}
```

## Running the Application

```bash
dotnet run --project SignalR.Events.Api
```

## Connecting to SignalR Hub (Client Example)

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5001/eventhub")
    .build();

connection.on("ReceiveEvent", (notification) => {
    console.log("Event received:", notification);
});

await connection.start();
```

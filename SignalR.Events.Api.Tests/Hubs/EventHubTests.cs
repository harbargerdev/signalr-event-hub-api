using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using SignalR.Events.Api.Hubs;
using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Tests.Hubs;

[TestFixture]
public class EventHubTests
{
    private Mock<ILogger<EventHub>> _mockLogger;
    private Mock<HubCallerContext> _mockContext;
    private Mock<IHubCallerClients> _mockClients;
    private Mock<IClientProxy> _mockClientProxy;
    private EventHub _hub;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<EventHub>>();
        _mockContext = new Mock<HubCallerContext>();
        _mockClients = new Mock<IHubCallerClients>();
        _mockClientProxy = new Mock<IClientProxy>();

        _mockContext.Setup(c => c.ConnectionId).Returns("test-connection-id");
        _mockClients.Setup(c => c.All).Returns(_mockClientProxy.Object);

        _hub = new EventHub(_mockLogger.Object)
        {
            Context = _mockContext.Object,
            Clients = _mockClients.Object
        };
    }

    [TearDown]
    public void TearDown()
    {
        _hub?.Dispose();
    }

    [Test]
    public async Task OnConnectedAsync_LogsConnectionInformation()
    {
        // Act
        await _hub.OnConnectedAsync();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("test-connection-id")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task OnDisconnectedAsync_LogsDisconnectionInformation()
    {
        // Act
        await _hub.OnDisconnectedAsync(null);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("test-connection-id")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task OnDisconnectedAsync_WithException_LogsDisconnectionInformation()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        await _hub.OnDisconnectedAsync(exception);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("test-connection-id")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    //[Test]
    public async Task BroadcastEvent_SendsEventToAllClients()
    {
        // Arrange
        var notification = new EventNotification
        {
            UserId = "test-user",
            EventType = "newEvent"
        };

        // Act
        await _hub.BroadcastEvent(notification);

        // Assert
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "ReceiveEvent",
                It.Is<object[]>(o => o.Length == 1 && o[0] == notification),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task BroadcastEvent_LogsEventInformation()
    {
        // Arrange
        var notification = new EventNotification
        {
            UserId = "test-user",
            EventType = "newEvent"
        };

        // Act
        await _hub.BroadcastEvent(notification);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => 
                    v.ToString()!.Contains("test-user") && 
                    v.ToString()!.Contains("newEvent")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    //[Test]
    public async Task BroadcastEvent_WithDifferentEventTypes_SendsCorrectEvent()
    {
        // Arrange
        var notification = new EventNotification
        {
            UserId = "test-user",
            EventType = "customEvent"
        };

        // Act
        await _hub.BroadcastEvent(notification);

        // Assert
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "ReceiveEvent",
                It.Is<object[]>(o => 
                    o.Length == 1 && 
                    ((EventNotification)o[0]).EventType == "customEvent"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using SignalR.Events.Api.Hubs;
using SignalR.Events.Api.Models;
using SignalR.Events.Api.Services;

namespace SignalR.Events.Api.Tests.Services;

[TestFixture]
public class CurrentUserEventWorkerTests
{
    private Mock<IHubContext<EventHub>> _mockHubContext;
    private Mock<ILogger<CurrentUserEventWorker>> _mockLogger;
    private Mock<IHubClients> _mockClients;
    private Mock<IClientProxy> _mockClientProxy;
    private CurrentUserEventWorker _worker;

    [SetUp]
    public void SetUp()
    {
        _mockHubContext = new Mock<IHubContext<EventHub>>();
        _mockLogger = new Mock<ILogger<CurrentUserEventWorker>>();
        _mockClients = new Mock<IHubClients>();
        _mockClientProxy = new Mock<IClientProxy>();

        _mockHubContext.Setup(h => h.Clients).Returns(_mockClients.Object);
        _mockClients.Setup(c => c.All).Returns(_mockClientProxy.Object);

        _worker = new CurrentUserEventWorker(_mockHubContext.Object, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _worker?.Dispose();
    }

    [Test]
    public async Task ExecuteAsync_StartsWorker_LogsStartupMessage()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        try
        {
            await _worker.StartAsync(cts.Token);
            await Task.Delay(150);
        }
        catch (TaskCanceledException)
        {
            // Expected when cancellation token fires
        }
        finally
        {
            await _worker.StopAsync(CancellationToken.None);
        }

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("started")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task ExecuteAsync_SendsEventWithEnvironmentUserName()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));
        var expectedUserName = Environment.UserName;

        // Act
        try
        {
            await _worker.StartAsync(cts.Token);
            await Task.Delay(150);
        }
        catch (TaskCanceledException)
        {
            // Expected when cancellation token fires
        }
        finally
        {
            await _worker.StopAsync(CancellationToken.None);
        }

        // Assert
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "ReceiveEvent",
                It.Is<object[]>(o => 
                    o.Length == 1 && 
                    o[0] is EventNotification && 
                    ((EventNotification)o[0]).UserId == expectedUserName),
                It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task ExecuteAsync_SendsEventWithNewEventType()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        try
        {
            await _worker.StartAsync(cts.Token);
            await Task.Delay(150);
        }
        catch (TaskCanceledException)
        {
            // Expected when cancellation token fires
        }
        finally
        {
            await _worker.StopAsync(CancellationToken.None);
        }

        // Assert
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "ReceiveEvent",
                It.Is<object[]>(o => 
                    o.Length == 1 && 
                    o[0] is EventNotification && 
                    ((EventNotification)o[0]).EventType == "newEvent"),
                It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task ExecuteAsync_LogsEventBroadcast()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        try
        {
            await _worker.StartAsync(cts.Token);
            await Task.Delay(150);
        }
        catch (TaskCanceledException)
        {
            // Expected when cancellation token fires
        }
        finally
        {
            await _worker.StopAsync(CancellationToken.None);
        }

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => 
                    v.ToString()!.Contains("Event broadcast") && 
                    v.ToString()!.Contains("current user")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task ExecuteAsync_WhenStopped_LogsStopMessage()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        try
        {
            await _worker.StartAsync(cts.Token);
            await Task.Delay(150);
        }
        catch (TaskCanceledException)
        {
            // Expected when cancellation token fires
        }
        finally
        {
            await _worker.StopAsync(CancellationToken.None);
        }

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("stopped")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}

using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Tests.Models;

[TestFixture]
public class EventNotificationTests
{
    [Test]
    public void EventNotification_DefaultConstructor_InitializesWithEmptyStrings()
    {
        // Act
        var notification = new EventNotification();

        // Assert
        Assert.That(notification.UserId, Is.EqualTo(string.Empty));
        Assert.That(notification.EventType, Is.EqualTo(string.Empty));
    }

    [Test]
    public void EventNotification_CanSetUserId()
    {
        // Arrange
        var notification = new EventNotification();
        var expectedUserId = "test-user-123";

        // Act
        notification.UserId = expectedUserId;

        // Assert
        Assert.That(notification.UserId, Is.EqualTo(expectedUserId));
    }

    [Test]
    public void EventNotification_CanSetEventType()
    {
        // Arrange
        var notification = new EventNotification();
        var expectedEventType = "customEvent";

        // Act
        notification.EventType = expectedEventType;

        // Assert
        Assert.That(notification.EventType, Is.EqualTo(expectedEventType));
    }

    [Test]
    public void EventNotification_InitializerSyntax_SetsAllProperties()
    {
        // Arrange
        var userId = "user-789";
        var eventType = "newEvent";

        // Act
        var notification = new EventNotification
        {
            UserId = userId,
            EventType = eventType
        };

        // Assert
        Assert.That(notification.UserId, Is.EqualTo(userId));
        Assert.That(notification.EventType, Is.EqualTo(eventType));
    }

    [Test]
    public void EventNotification_WithNewEventType_CreatesValidNotification()
    {
        // Arrange & Act
        var notification = new EventNotification
        {
            UserId = "current-user",
            EventType = "newEvent"
        };

        // Assert
        Assert.That(notification.UserId, Is.EqualTo("current-user"));
        Assert.That(notification.EventType, Is.EqualTo("newEvent"));
    }

    [Test]
    public void EventNotification_CanHandleEmptyUserId()
    {
        // Arrange & Act
        var notification = new EventNotification
        {
            UserId = string.Empty,
            EventType = "newEvent"
        };

        // Assert
        Assert.That(notification.UserId, Is.EqualTo(string.Empty));
        Assert.That(notification.EventType, Is.EqualTo("newEvent"));
    }

    [Test]
    public void EventNotification_CanHandleNullValues()
    {
        // Arrange & Act
        var notification = new EventNotification
        {
            UserId = null!,
            EventType = null!
        };

        // Assert
        Assert.That(notification.UserId, Is.Null);
        Assert.That(notification.EventType, Is.Null);
    }
}

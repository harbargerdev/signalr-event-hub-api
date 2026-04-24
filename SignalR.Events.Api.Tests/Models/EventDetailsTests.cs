using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Tests.Models;

[TestFixture]
public class EventDetailsTests
{
    [Test]
    public void EventDetails_DefaultConstructor_InitializesWithEmptyStrings()
    {
        // Act
        var eventDetails = new EventDetails();

        // Assert
        Assert.That(eventDetails.UserId, Is.EqualTo(string.Empty));
        Assert.That(eventDetails.EventName, Is.EqualTo(string.Empty));
        Assert.That(eventDetails.EventDescription, Is.EqualTo(string.Empty));
    }

    [Test]
    public void EventDetails_DefaultConstructor_GeneratesValidGuid()
    {
        // Act
        var eventDetails = new EventDetails();

        // Assert
        Assert.That(eventDetails.EventId, Is.Not.Null);
        Assert.That(Guid.TryParse(eventDetails.EventId, out _), Is.True);
    }

    [Test]
    public void EventDetails_MultipleInstances_GenerateUniqueEventIds()
    {
        // Act
        var eventDetails1 = new EventDetails();
        var eventDetails2 = new EventDetails();

        // Assert
        Assert.That(eventDetails1.EventId, Is.Not.EqualTo(eventDetails2.EventId));
    }

    [Test]
    public void EventDetails_CanSetUserId()
    {
        // Arrange
        var eventDetails = new EventDetails();
        var expectedUserId = "test-user-123";

        // Act
        eventDetails.UserId = expectedUserId;

        // Assert
        Assert.That(eventDetails.UserId, Is.EqualTo(expectedUserId));
    }

    [Test]
    public void EventDetails_CanSetEventId()
    {
        // Arrange
        var eventDetails = new EventDetails();
        var expectedEventId = Guid.NewGuid().ToString();

        // Act
        eventDetails.EventId = expectedEventId;

        // Assert
        Assert.That(eventDetails.EventId, Is.EqualTo(expectedEventId));
    }

    [Test]
    public void EventDetails_CanSetEventName()
    {
        // Arrange
        var eventDetails = new EventDetails();
        var expectedEventName = "Test Event";

        // Act
        eventDetails.EventName = expectedEventName;

        // Assert
        Assert.That(eventDetails.EventName, Is.EqualTo(expectedEventName));
    }

    [Test]
    public void EventDetails_CanSetEventDescription()
    {
        // Arrange
        var eventDetails = new EventDetails();
        var expectedDescription = "This is a test event description";

        // Act
        eventDetails.EventDescription = expectedDescription;

        // Assert
        Assert.That(eventDetails.EventDescription, Is.EqualTo(expectedDescription));
    }

    [Test]
    public void EventDetails_InitializerSyntax_SetsAllProperties()
    {
        // Arrange
        var userId = "user-456";
        var eventId = Guid.NewGuid().ToString();
        var eventName = "Sample Event";
        var eventDescription = "Sample Description";

        // Act
        var eventDetails = new EventDetails
        {
            UserId = userId,
            EventId = eventId,
            EventName = eventName,
            EventDescription = eventDescription
        };

        // Assert
        Assert.That(eventDetails.UserId, Is.EqualTo(userId));
        Assert.That(eventDetails.EventId, Is.EqualTo(eventId));
        Assert.That(eventDetails.EventName, Is.EqualTo(eventName));
        Assert.That(eventDetails.EventDescription, Is.EqualTo(eventDescription));
    }
}

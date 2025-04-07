using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.UnitTest.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Attendance.UnitTest.Events;

#pragma warning disable CA1515
public class EventTests : BaseTest
#pragma warning restore CA1515
{
    [Fact]
    public void Should_RaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        DateTime startsAtUtc = DateTime.Now;

        // Act
        Result<Event> result = Event.Create(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        // Assert
        EventCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCreatedDomainEvent>(result.Value);
        
        domainEvent.EventId.Should().Be(result.Value.Id);
    }
}

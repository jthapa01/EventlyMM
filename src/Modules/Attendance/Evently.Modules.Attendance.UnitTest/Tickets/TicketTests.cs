using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.UnitTest.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Attendance.UnitTest.Tickets;

#pragma warning disable CA1515
public class TicketTests : BaseTest
#pragma warning restore CA1515
{
    [Fact]
    public void Should_RaiseDomainEvent_WhenTicketCreated()
    {
        // Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);
        
        DateTime startsAtUtc = DateTime.Now;
        
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetName(),
            startsAtUtc, null);
        
        // Act
        Result<Ticket> result = Ticket.Create(
            Guid.NewGuid(),
            attendee,
            @event,
            Faker.Random.String());

        // Assert
        TicketCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);
        
        domainEvent.TicketId.Should().Be(result.Value.Id);
    }
}

using Evently.Common.Domain;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Attendance.IntegrationTests.Events;

public class CreateEventTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<Guid, string, string, string, DateTime, DateTime?> InvalidData =
        new()
        {
            { Guid.Empty, Faker.Music.Genre(), Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default },
            { Guid.NewGuid(), string.Empty, Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default },
            { Guid.NewGuid(), Faker.Music.Genre(), string.Empty, Faker.Address.StreetAddress(), default, default },
            { Guid.NewGuid(), Faker.Music.Genre(), Faker.Music.Genre(), string.Empty, default, default },
            { Guid.NewGuid(), Faker.Music.Genre(), Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default }
        };
    
    [Theory]
    [MemberData(nameof(InvalidData))]
    public async Task CreateEvent_InvalidData_ReturnsBadRequest(
        Guid eventId,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        // Arrange
        var command = new CreateEventCommand(eventId, title, description, location, startsAtUtc, endsAtUtc);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public async Task Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateEventCommand(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1));

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
}

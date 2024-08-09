using Evently.Modules.Events.Application.Abstractions.Clock;

namespace Evently.Modules.Events.Infrastructure.Clock;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

namespace Evently.Common.Application.EventBus;

public abstract class IntegrationEvent(Guid id, DateTime occurredOnUtc) : IIntegrationEvent
{
    public Guid Id { get; } = id;
    public DateTime OccurredOnUtc { get; } = occurredOnUtc;
}

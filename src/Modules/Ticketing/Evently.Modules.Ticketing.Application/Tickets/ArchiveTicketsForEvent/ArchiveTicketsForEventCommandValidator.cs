using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;

public sealed class ArchiveTicketsForEventCommandValidator : AbstractValidator<ArchiveTicketsForEventCommand>
{
    public ArchiveTicketsForEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}

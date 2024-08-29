using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Events.CancelEvent;

public sealed class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
{
    public CancelEventCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty();
    }
}

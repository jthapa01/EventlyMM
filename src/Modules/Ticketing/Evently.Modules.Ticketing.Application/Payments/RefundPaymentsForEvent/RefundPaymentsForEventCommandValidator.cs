using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

public sealed class RefundPaymentsForEventCommandValidator: AbstractValidator<RefundPaymentsForEventCommand>
{
    public RefundPaymentsForEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}

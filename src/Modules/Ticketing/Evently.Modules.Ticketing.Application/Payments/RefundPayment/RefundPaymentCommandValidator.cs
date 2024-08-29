using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPayment;

public class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        RuleFor(c => c.PaymentId).NotEmpty();
    }
}

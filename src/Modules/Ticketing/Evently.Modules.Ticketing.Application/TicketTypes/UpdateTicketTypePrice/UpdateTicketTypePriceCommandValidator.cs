using FluentValidation;

namespace Evently.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;

public sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(c => c.TicketTypeId).NotEmpty();
        RuleFor(c => c.Price).GreaterThan(decimal.Zero);
    }
}

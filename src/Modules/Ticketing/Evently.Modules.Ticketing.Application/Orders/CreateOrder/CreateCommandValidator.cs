using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

public class CreateCommandValidator: AbstractValidator<CreateOrderCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}

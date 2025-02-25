using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

public class CreateOrderCommandValidator: AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}

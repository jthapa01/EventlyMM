using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.ClearCart;

public sealed class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}
